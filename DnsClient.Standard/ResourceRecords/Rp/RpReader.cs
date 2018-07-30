namespace DnsClient.Standard.ResourceRecords.Rp
{
    using Core;

    public sealed class RpReader : IResourceRecordReader<RpRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = RpRecord.ResourceRecordType;

        public RpRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new RpRecord(info, reader.ReadDnsName(), reader.ReadDnsName());
    }
}