namespace DnsClient.Standard.ResourceRecords.Mx
{
    using Core;

    public sealed class MxReader : IResourceRecordReader<MxRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = MxRecord.ResourceRecordType;

        public MxRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader)
        {
            var preference = reader.ReadUInt16NetworkOrder();
            var domain = reader.ReadDnsName();

            return new MxRecord(info, preference, domain);
        }
    }
}