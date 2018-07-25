namespace DnsClient.ResourceRecords.Mr
{
    using Core;
    using Core.ResourceRecords;

    public sealed class MrReader : IResourceRecordReader<MrRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Mr;

        public MrRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new MrRecord(info, reader.ReadDnsName());
    }
}