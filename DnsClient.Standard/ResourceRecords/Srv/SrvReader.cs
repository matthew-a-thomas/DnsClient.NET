namespace DnsClient.Standard.ResourceRecords.Srv
{
    using Core;

    public sealed class SrvReader : IResourceRecordReader<SrvRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = SrvRecord.ResourceRecordType;

        public SrvRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader)
        {
            var priority = reader.ReadUInt16NetworkOrder();
            var weight = reader.ReadUInt16NetworkOrder();
            var port = reader.ReadUInt16NetworkOrder();
            var target = reader.ReadDnsName();

            return new SrvRecord(info, priority, weight, port, target);
        }
    }
}