namespace DnsClient.Standard.ResourceRecords.CName
{
    using Core;

    public sealed class CNameReader : IResourceRecordReader<CNameRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = CNameRecord.ResourceRecordType;

        public CNameRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new CNameRecord(info, reader.ReadDnsName());
    }
}