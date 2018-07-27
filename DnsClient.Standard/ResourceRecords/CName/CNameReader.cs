namespace DnsClient.Standard.ResourceRecords.CName
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class CNameReader : IResourceRecordReader<CNameRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = CNameRecord.ResourceRecordType;

        public CNameRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new CNameRecord(info, reader.ReadDnsName());
    }
}