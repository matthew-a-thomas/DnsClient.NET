namespace DnsClient.ResourceRecords.A
{
    using Core;
    using Core.Protocol;
    public sealed class AReader : IResourceRecordReader<ARecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.A;

        public ARecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new ARecord(info, reader.ReadIpAddress());
    }
}