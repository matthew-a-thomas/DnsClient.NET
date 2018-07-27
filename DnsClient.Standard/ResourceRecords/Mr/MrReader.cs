namespace DnsClient.Standard.ResourceRecords.Mr
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class MrReader : IResourceRecordReader<MrRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = MrRecord.ResourceRecordType;

        public MrRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new MrRecord(info, reader.ReadDnsName());
    }
}