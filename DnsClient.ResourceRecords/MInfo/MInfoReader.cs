namespace DnsClient.ResourceRecords.MInfo
{
    using Core;

    public sealed class MInfoReader : IResourceRecordReader<MInfoRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Minfo;

        public MInfoRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new MInfoRecord(info, reader.ReadDnsName(), reader.ReadDnsName());
    }
}