namespace DnsClient.ResourceRecords.CName
{
    using Core;

    public sealed class CNameReader : IResourceRecordReader<CNameRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Cname;

        public CNameRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new CNameRecord(info, reader.ReadDnsName());
    }
}