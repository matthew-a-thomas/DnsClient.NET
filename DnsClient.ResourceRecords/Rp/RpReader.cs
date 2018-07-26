namespace DnsClient.ResourceRecords.Rp
{
    using Core;

    public sealed class RpReader : IResourceRecordReader<RpRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Rp;

        public RpRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new RpRecord(info, reader.ReadDnsName(), reader.ReadDnsName());
    }
}