﻿namespace DnsClient.Core
{
    using System;
    using System.Buffers;

    public sealed class PooledBytes : IDisposable
    {
        private static readonly ArrayPool<byte> Pool = ArrayPool<byte>.Create(4096 * 2, 200);

        private readonly byte[] _buffer;
        private bool _disposed;

        public PooledBytes(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            _buffer = Pool.Rent(length);
        }

        public byte[] Buffer
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(Buffer));
                }

                return _buffer;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_disposed)
                    return;

                Pool.Return(_buffer);
                _disposed = true;
            }
        }
    }
}