namespace DnsClient.Standard.ResourceRecords.RrSig
{
    using DnsClient.ResourceRecords;

    public static class RrSigResourceRecordType
    {
        /// <summary>
        /// RRSIG rfc3755.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3755">RFC 3755</seealso>
        public static readonly PseudoResourceRecordType Instance = new PseudoResourceRecordType(
            abbreviation: "RrSig",
            value: 46
        );
    }
}