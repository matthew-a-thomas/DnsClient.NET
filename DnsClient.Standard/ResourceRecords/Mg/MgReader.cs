namespace DnsClient.Standard.ResourceRecords.Mg
{
    using Core;

    public sealed class MgReader : IResourceRecordReader<MgRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = MgRecord.ResourceRecordType;

        public MgRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new MgRecord(info, reader.ReadDnsName());
    }
}