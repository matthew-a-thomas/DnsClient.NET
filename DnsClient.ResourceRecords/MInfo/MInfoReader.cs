namespace DnsClient.ResourceRecords.MInfo
{
    using Core;
    using Core.ResourceRecords;

    public sealed class MInfoReader : IResourceRecordReader<MInfoRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Minfo;

        public MInfoRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new MInfoRecord(info, reader.ReadDnsName(), reader.ReadDnsName());
    }
}