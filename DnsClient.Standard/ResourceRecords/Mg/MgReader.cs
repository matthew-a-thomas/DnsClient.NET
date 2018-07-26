namespace DnsClient.Standard.ResourceRecords.Mg
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class MgReader : IResourceRecordReader<MgRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Mg;

        public MgRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new MgRecord(info, reader.ReadDnsName());
    }
}