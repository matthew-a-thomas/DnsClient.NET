namespace DnsClient.ResourceRecords.Ptr
{
    using Core;

    public sealed class PtrReader : IResourceRecordReader<PtrRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Ptr;

        public PtrRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new PtrRecord(info, reader.ReadDnsName());
    }
}