namespace DnsClient.Standard.ResourceRecords.MInfo
{
    using Core;

    public sealed class MInfoReader : IResourceRecordReader<MInfoRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = MInfoRecord.ResourceRecordType;

        public MInfoRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new MInfoRecord(info, reader.ReadDnsName(), reader.ReadDnsName());
    }
}