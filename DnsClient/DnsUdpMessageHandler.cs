namespace DnsClient
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;

    internal class DnsUdpMessageHandler : DnsMessageHandler
    {
        private const int MaxSize = 4096;
        private static readonly ConcurrentQueue<UdpClient> Clients = new ConcurrentQueue<UdpClient>();
        private static readonly ConcurrentQueue<UdpClient> ClientsIPv6 = new ConcurrentQueue<UdpClient>();
        private readonly bool _enableClientQueue;

        public DnsUdpMessageHandler(bool enableClientQueue)
        {
            _enableClientQueue = enableClientQueue;
        }

        public override bool IsTransientException<T>(T exception)
        {
            if (exception is SocketException) return true;
            return false;
        }

        public override DnsResponseMessage Query(
            IPEndPoint server,
            DnsRequestMessage request,
            TimeSpan timeout)
        {
            var udpClient = GetNextUdpClient(server.AddressFamily);

            // -1 indicates infinite
            var timeoutInMillis = timeout.TotalMilliseconds >= int.MaxValue ? -1 : (int)timeout.TotalMilliseconds;
            udpClient.Client.ReceiveTimeout = timeoutInMillis;
            udpClient.Client.SendTimeout = timeoutInMillis;

            var mustDispose = false;
            try
            {
                using (var writer = new DnsDatagramWriter())
                {
                    GetRequestData(request, writer);
                    Debug.Assert(writer.Data.Array != null);
                    udpClient.Client.SendTo(writer.Data.Array, writer.Data.Offset, writer.Data.Count, SocketFlags.None, server);
                }

                var readSize = udpClient.Available > MaxSize ? udpClient.Available : MaxSize;

                using (var memory = new PooledBytes(readSize))
                {
                    var received = udpClient.Client.Receive(memory.Buffer, 0, readSize, SocketFlags.None);

                    var response = GetResponseMessage(new ArraySegment<byte>(memory.Buffer, 0, received));
                    if (request.Header.Id != response.Header.Id)
                    {
                        throw new DnsResponseException("Header id mismatch.");
                    }

                    Enqueue(server.AddressFamily, udpClient);

                    return response;
                }
            }
            catch
            {
                mustDispose = true;
                throw;
            }
            finally
            {
                if (!_enableClientQueue || mustDispose)
                {
                    try
                    {
                        udpClient.Close();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        public override async Task<DnsResponseMessage> QueryAsync(
            IPEndPoint server,
            DnsRequestMessage request,
            CancellationToken cancellationToken,
            Action<Action> cancelationCallback)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var udpClient = GetNextUdpClient(server.AddressFamily);

            var mustDispose = false;
            try
            {
                // setup timeout cancelation, dispose socket (the only way to acutally cancel the request in async...
                cancelationCallback(() =>
                {
                    udpClient.Close();
                });

                using (var writer = new DnsDatagramWriter())
                {
                    GetRequestData(request, writer);
                    await udpClient.SendAsync(writer.Data.Array, writer.Data.Count, server).ConfigureAwait(false);
                }

                var result = await udpClient.ReceiveAsync().ConfigureAwait(false);

                var response = GetResponseMessage(new ArraySegment<byte>(result.Buffer, 0, result.Buffer.Length));

                if (request.Header.Id != response.Header.Id)
                {
                    throw new DnsResponseException("Header id mismatch.");
                }

                Enqueue(server.AddressFamily, udpClient);

                return response;
            }
            catch (ObjectDisposedException)
            {
                // we disposed it in case of a timeout request, lets indicate it actually timed out...
                throw new TimeoutException();
            }
            catch
            {
                mustDispose = true;

                throw;
            }
            finally
            {
                if (!_enableClientQueue || mustDispose)
                {
                    try
                    {
                        udpClient.Close();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        private UdpClient GetNextUdpClient(AddressFamily family)
        {
            UdpClient udpClient = null;
            if (_enableClientQueue)
            {
                while (udpClient == null && !TryDequeue(family, out udpClient))
                {
                    ////Interlocked.Increment(ref StaticLog.CreatedClients);
                    udpClient = new UdpClient(family);
                }
            }
            else
            {
                ////Interlocked.Increment(ref StaticLog.CreatedClients);
                udpClient = new UdpClient(family);
            }

            return udpClient;
        }

        private void Enqueue(AddressFamily family, UdpClient client)
        {
            if (_enableClientQueue)
            {
                if (family == AddressFamily.InterNetwork)
                {
                    Clients.Enqueue(client);
                }
                else
                {
                    ClientsIPv6.Enqueue(client);
                }
            }
        }

        private bool TryDequeue(AddressFamily family, out UdpClient client)
        {
            if (family == AddressFamily.InterNetwork)
            {
                return Clients.TryDequeue(out client);
            }

            return ClientsIPv6.TryDequeue(out client);
        }
    }
}