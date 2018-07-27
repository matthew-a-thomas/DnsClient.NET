namespace DnsClient.Standard.ResourceRecords.MInfo
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class MInfoReader : IResourceRecordReader<MInfoRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = MInfoRecord.ResourceRecordType;

        public MInfoRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new MInfoRecord(info, reader.ReadDnsName(), reader.ReadDnsName());
    }
}