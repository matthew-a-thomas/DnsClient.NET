namespace DnsClient.ResourceRecords.Aaaa
{
    using Core;

    public sealed class AaaaReader : IResourceRecordReader<AaaaRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Aaaa;

        public AaaaRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new AaaaRecord(info, reader.ReadIPv6Address());
    }
}