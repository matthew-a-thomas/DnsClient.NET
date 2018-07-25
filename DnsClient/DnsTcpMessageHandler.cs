namespace DnsClient
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;

    internal class DnsTcpMessageHandler : DnsMessageHandler
    {
        private readonly ConcurrentDictionary<IPEndPoint, ClientPool> _pools = new ConcurrentDictionary<IPEndPoint, ClientPool>();

        public override bool IsTransientException<T>(T exception)
        {
            //if (exception is SocketException) return true;
            return false;
        }

        public override DnsResponseMessage Query(IPEndPoint endpoint, DnsRequestMessage request, TimeSpan timeout)
        {
            if (timeout != Timeout.InfiniteTimeSpan && timeout.TotalMilliseconds < int.MaxValue)
            {
                using (var cts = new CancellationTokenSource(timeout))
                {
                    Action onCancel = () => { };
                    return QueryAsync(endpoint, request, cts.Token, s => onCancel = s)
                        .WithCancellation(cts.Token, onCancel)
                        .ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }

            return QueryAsync(endpoint, request, CancellationToken.None, s => { }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public override async Task<DnsResponseMessage> QueryAsync(
            IPEndPoint server,
            DnsRequestMessage request,
            CancellationToken cancellationToken,
            Action<Action> cancelationCallback)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ClientPool pool;
            ClientPool.ClientEntry entry = null;
            while (!_pools.TryGetValue(server, out pool))
            {
                _pools.TryAdd(server, new ClientPool(true, server));
            }

            cancelationCallback(() =>
            {
                if (entry == null) return;
                entry.DisposeClient();
            });

            DnsResponseMessage response = null;

            while (response == null)
            {
                entry = await pool.GetNexClient().ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();

                response = await QueryAsyncInternal(
                        entry.Client.GetStream(), request, cancellationToken)
                    .ConfigureAwait(false);

                if (response != null)
                {
                    pool.Enqueue(entry);
                }
                else
                {
                    entry.DisposeClient();
                }
            }

            return response;
        }

        private async Task<DnsResponseMessage> QueryAsyncInternal(
            Stream stream,
            DnsRequestMessage request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // use a pooled buffer to writer the data + the length of the data later into the frist two bytes
            using (var memory = new PooledBytes(DnsDatagramWriter.BufferSize + 2))
            using (var writer = new DnsDatagramWriter(new ArraySegment<byte>(memory.Buffer, 2, memory.Buffer.Length - 2)))
            {
                GetRequestData(request, writer);
                var dataLength = writer.Index;
                memory.Buffer[0] = (byte)((dataLength >> 8) & 0xff);
                memory.Buffer[1] = (byte)(dataLength & 0xff);

                //await client.Client.SendAsync(new ArraySegment<byte>(memory.Buffer, 0, dataLength + 2), SocketFlags.None).ConfigureAwait(false);
                await stream.WriteAsync(memory.Buffer, 0, dataLength + 2, cancellationToken).ConfigureAwait(false);
                await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
            }

            if (!stream.CanRead)
            {
                return null;
            }

            cancellationToken.ThrowIfCancellationRequested();

            int length;
            try
            {
                length = stream.ReadByte() << 8 | stream.ReadByte();
            }
            catch (Exception ex) when (ex is IOException || ex is SocketException)
            {
                return null;
            }

            if (length <= 0)
            {
                // server signals close/disconnecting
                return null;
            }

            using (var memory = new PooledBytes(length))
            {
                var bytesReceived = 0;
                var readSize = length > 4096 ? 4096 : length;

                while ((bytesReceived += await stream.ReadAsync(memory.Buffer, bytesReceived, readSize).ConfigureAwait(false)) < length)
                {
                    if (bytesReceived <= 0)
                    {
                        // disconnected
                        return null;
                    }
                    if (bytesReceived + readSize > length)
                    {
                        readSize = length - bytesReceived;

                        if (readSize <= 0)
                        {
                            break;
                        }
                    }
                }

                var response = GetResponseMessage(new ArraySegment<byte>(memory.Buffer, 0, bytesReceived));
                if (request.Header.Id != response.Header.Id)
                {
                    throw new DnsResponseException("Header id mismatch.");
                }

                return response;
            }
        }

        private class ClientPool : IDisposable
        {
            private bool _disposedValue;
            private readonly bool _enablePool;
            private ConcurrentQueue<ClientEntry> _clients = new ConcurrentQueue<ClientEntry>();
            private readonly IPEndPoint _endpoint;

            public ClientPool(bool enablePool, IPEndPoint endpoint)
            {
                _enablePool = enablePool;
                _endpoint = endpoint;
            }

            public async Task<ClientEntry> GetNexClient()
            {
                if (_disposedValue) throw new ObjectDisposedException(nameof(ClientPool));

                ClientEntry entry = null;
                if (_enablePool)
                {
                    while (entry == null && !TryDequeue(out entry))
                    {
                        ////Interlocked.Increment(ref StaticLog.CreatedClients);
                        entry = new ClientEntry(new TcpClient(_endpoint.AddressFamily) { LingerState = new LingerOption(true, 0) });
                        await entry.Client.ConnectAsync(_endpoint.Address, _endpoint.Port).ConfigureAwait(false);
                    }
                }
                else
                {
                    ////Interlocked.Increment(ref StaticLog.CreatedClients);
                    entry = new ClientEntry(new TcpClient(_endpoint.AddressFamily));
                    await entry.Client.ConnectAsync(_endpoint.Address, _endpoint.Port).ConfigureAwait(false);
                }

                return entry;
            }

            public void Enqueue(ClientEntry entry)
            {
                if (_disposedValue) throw new ObjectDisposedException(nameof(ClientPool));
                if (entry == null) throw new ArgumentNullException(nameof(entry));
                if (!entry.Client.Client.RemoteEndPoint.Equals(_endpoint)) throw new ArgumentException("Invalid endpoint.");

                // TickCount swap will be fine here as the entry just gets disposed and we'll create a new one starting at 0+ again, totall fine...
                if (_enablePool && entry.Client.Connected && entry.StartMillis + entry.MaxLiveTime >= (Environment.TickCount & int.MaxValue))
                {
                    _clients.Enqueue(entry);
                }
                else
                {
                    // dispose the client and don't keep a reference
                    entry.DisposeClient();
                }
            }

            private bool TryDequeue(out ClientEntry entry)
            {
                if (_disposedValue) throw new ObjectDisposedException(nameof(ClientPool));

                bool result;
                // ReSharper disable once AssignmentInConditionalExpression
                while (result = _clients.TryDequeue(out entry))
                {
                    // validate the client before returning it
                    if (entry.Client.Connected && entry.StartMillis + entry.MaxLiveTime >= (Environment.TickCount & int.MaxValue))
                    {
                        break;
                    }

                    entry.DisposeClient();
                }

                return result;
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposedValue)
                {
                    if (disposing)
                    {
                        foreach (var entry in _clients)
                        {
                            entry.DisposeClient();
                        }

                        _clients = new ConcurrentQueue<ClientEntry>();
                    }

                    _disposedValue = true;
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            public class ClientEntry
            {
                public ClientEntry(TcpClient client)
                {
                    Client = client;
                }

                public void DisposeClient()
                {
                    try
                    {
                        Client.Close();
                    }
                    catch
                    {
                        // ignored
                    }
                }

                public TcpClient Client { get; }

                public int StartMillis { get; } = Environment.TickCount & int.MaxValue;

                public int MaxLiveTime { get; } = 5000;
            }
        }
    }
}