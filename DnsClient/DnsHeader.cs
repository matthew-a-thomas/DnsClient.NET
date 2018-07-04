namespace DnsClient
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "ConvertToConstant.Global")] // Otherwise, results in "overflow in constant" warnings.
    internal static class DnsHeader
    {
        public static readonly ushort OpCodeMask = 0x7800;
        public static readonly ushort OpCodeShift = 11;
        public static readonly ushort RCodeMask = 0x000F;
    }
}