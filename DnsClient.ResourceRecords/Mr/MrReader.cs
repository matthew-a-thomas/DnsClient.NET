namespace DnsClient.ResourceRecords.Mr
{
    using Core;

    public sealed class MrReader : IResourceRecordReader<MrRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Mr;

        public MrRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new MrRecord(info, reader.ReadDnsName());
    }
}