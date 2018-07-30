namespace DnsClient.Standard.ResourceRecords.Mr
{
    using Core;

    public sealed class MrReader : IResourceRecordReader<MrRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = MrRecord.ResourceRecordType;

        public MrRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new MrRecord(info, reader.ReadDnsName());
    }
}