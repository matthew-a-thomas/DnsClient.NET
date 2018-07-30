namespace DnsClient.Standard.ResourceRecords.Soa
{
    using Core;

    public sealed class SoaReader : IResourceRecordReader<SoaRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = SoaRecord.ResourceRecordType;

        public SoaRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader)
        {
            var mName = reader.ReadDnsName();
            var rName = reader.ReadDnsName();
            var serial = reader.ReadUInt32NetworkOrder();
            var refresh = reader.ReadUInt32NetworkOrder();
            var retry = reader.ReadUInt32NetworkOrder();
            var expire = reader.ReadUInt32NetworkOrder();
            var minimum = reader.ReadUInt32NetworkOrder();

            return new SoaRecord(info, mName, rName, serial, refresh, retry, expire, minimum);
        }
    }
}