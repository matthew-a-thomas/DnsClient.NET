namespace DnsClient.Core
{
    using System;
    using System.Diagnostics;

    internal static class ArraySegmentExtensions
    {
        public static ArraySegment<T> SubArray<T>(this ArraySegment<T> array, int startIndex, int length)
        {
            Debug.Assert(array.Array != null);
            return new ArraySegment<T>(array.Array, array.Offset + startIndex, length);
        }

        public static ArraySegment<T> SubArrayFromOriginal<T>(this ArraySegment<T> array, int startIndex)
        {
            Debug.Assert(array.Array != null);
            return new ArraySegment<T>(array.Array, startIndex, array.Array.Length - startIndex);
        }
    }
}