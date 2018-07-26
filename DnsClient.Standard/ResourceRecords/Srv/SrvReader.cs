namespace DnsClient.Standard.ResourceRecords.Srv
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class SrvReader : IResourceRecordReader<SrvRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Srv;

        public SrvRecord ReadResourceRecord(
            ResourceRecordInfo info,
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