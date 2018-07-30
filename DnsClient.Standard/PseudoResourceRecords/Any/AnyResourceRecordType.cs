namespace DnsClient.Standard.PseudoResourceRecords.Any
{
    using DnsClient.ResourceRecords;

    public static class AnyResourceRecordType
    {
        /// <summary>
        /// Generic any query *.
        /// </summary>
        public static readonly PseudoResourceRecordType Instance = new PseudoResourceRecordType(
            abbreviation: "ANY",
            value: 255
        );
    }
}