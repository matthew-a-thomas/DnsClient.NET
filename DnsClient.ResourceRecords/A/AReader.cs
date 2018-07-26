namespace DnsClient.ResourceRecords.A
{
    using Core;

    public sealed class AReader : IResourceRecordReader<ARecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.A;

        public ARecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new ARecord(info, reader.ReadIpAddress());
    }
}