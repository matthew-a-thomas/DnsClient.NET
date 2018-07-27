namespace DnsClient.Standard.ResourceRecords.Mf
{
    using System;
    using DnsClient.ResourceRecords;

    public static class MfResourceRecordType
    {
        /// <summary>
        /// A mail forwarder (OBSOLETE - use MX).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        [Obsolete("Use MX")]
        public static readonly PseudoResourceRecordType Instance = new PseudoResourceRecordType(
            abbreviation: "Mf",
            value: 4
        );
    }
}