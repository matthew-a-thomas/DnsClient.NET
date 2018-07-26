namespace DnsClient.ResourceRecords.Mx
{
    using Core;

    public sealed class MxReader : IResourceRecordReader<MxRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Mx;

        public MxRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader)
        {
            var preference = reader.ReadUInt16NetworkOrder();
            var domain = reader.ReadDnsName();

            return new MxRecord(info, preference, domain);
        }
    }
}