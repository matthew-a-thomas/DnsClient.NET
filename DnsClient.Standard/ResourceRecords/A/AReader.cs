namespace DnsClient.Standard.ResourceRecords.A
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class AReader : IResourceRecordReader<ARecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.A;

        public ARecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new ARecord(info, reader.ReadIpAddress());
    }
}