namespace DnsClient.Core
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Text;

    public sealed class DnsDatagramWriter
    {
        // queries can only be 255 octets + some header bytes, so that size is pretty safe...
        public const int BufferSize = 1024;

        private const byte DotByte = 46;

        private readonly ArraySegment<byte> _buffer;

        public ArraySegment<byte> Data
        {
            get
            {
                Debug.Assert(_buffer.Array != null);
                return new ArraySegment<byte>(_buffer.Array, 0, Index);
            }
        }

        public int Index { get; set; }

        public DnsDatagramWriter(ArraySegment<byte> useBuffer)
        {
            Debug.Assert(useBuffer.Count >= BufferSize);

            _buffer = useBuffer;
        }

        public void WriteHostName(string queryName)
        {
            var bytes = Encoding.UTF8.GetBytes(queryName);
            var lastOctet = 0;
            var index = 0;
            if (bytes.Length <= 1)
            {
                WriteByte(0);
                return;
            }
            foreach (var b in bytes)
            {
                if (b == DotByte)
                {
                    WriteByte((byte)(index - lastOctet)); // length
                    WriteBytes(bytes, lastOctet, index - lastOctet);
                    lastOctet = index + 1;
                }

                index++;
            }

            WriteByte(0);
        }

        public void WriteByte(byte b)
        {
            Debug.Assert(_buffer.Array != null);
            _buffer.Array[_buffer.Offset + Index++] = b;
        }

        public void WriteBytes(byte[] data, int length) => WriteBytes(data, 0, length);

        public void WriteBytes(byte[] data, int dataOffset, int length)
        {
            Debug.Assert(_buffer.Array != null);
            Buffer.BlockCopy(data, dataOffset, _buffer.Array, _buffer.Offset + Index, length);

            Index += length;
        }

        public void WriteInt16NetworkOrder(short value)
        {
            var bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            WriteBytes(bytes, bytes.Length);
        }

        public void WriteInt32NetworkOrder(int value)
        {
            var bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            WriteBytes(bytes, bytes.Length);
        }

        public void WriteUInt16NetworkOrder(ushort value) => WriteInt16NetworkOrder((short)value);

        public void WriteUInt32NetworkOrder(uint value) => WriteInt32NetworkOrder((int)value);
    }
}