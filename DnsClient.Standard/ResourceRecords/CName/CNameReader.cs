namespace DnsClient.Standard.ResourceRecords.CName
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class CNameReader : IResourceRecordReader<CNameRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Cname;

        public CNameRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new CNameRecord(info, reader.ReadDnsName());
    }
}