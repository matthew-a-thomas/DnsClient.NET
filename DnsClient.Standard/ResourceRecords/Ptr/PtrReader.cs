namespace DnsClient.Standard.ResourceRecords.Ptr
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class PtrReader : IResourceRecordReader<PtrRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = PtrRecord.ResourceRecordType;

        public PtrRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new PtrRecord(info, reader.ReadDnsName());
    }
}