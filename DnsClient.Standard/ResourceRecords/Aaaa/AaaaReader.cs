namespace DnsClient.Standard.ResourceRecords.Aaaa
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class AaaaReader : IResourceRecordReader<AaaaRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = AaaaRecord.ResourceRecordType;

        public AaaaRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new AaaaRecord(info, reader.ReadIPv6Address());
    }
}