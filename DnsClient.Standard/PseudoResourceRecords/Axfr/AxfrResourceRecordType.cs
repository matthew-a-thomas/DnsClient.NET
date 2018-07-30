namespace DnsClient.Standard.PseudoResourceRecords.Axfr
{
    using DnsClient.ResourceRecords;

    public static class AxfrResourceRecordType
    {
        /// <summary>
        /// DNS zone transfer request.
        /// <c>AXFR</c> is only supported via TCP.
        /// <para>
        /// The DNS Server might only return results for the request if the client connection/IP is allowed to do so.
        /// </para>
        /// </summary>
        public static readonly PseudoResourceRecordType Instance = new PseudoResourceRecordType(
            abbreviation: "AXFR",
            value: 252
        );
    }
}