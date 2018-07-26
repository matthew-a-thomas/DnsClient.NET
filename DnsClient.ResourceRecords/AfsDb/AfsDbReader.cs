namespace DnsClient.ResourceRecords.AfsDb
{
    using Core;

    public sealed class AfsDbReader : IResourceRecordReader<AfsDbRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.AfsDb;

        public AfsDbRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new AfsDbRecord(
            info,
            (AfsType) reader.ReadUInt16NetworkOrder(),
            reader.ReadDnsName());
    }
}