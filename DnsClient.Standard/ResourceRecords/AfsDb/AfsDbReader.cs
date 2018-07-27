namespace DnsClient.Standard.ResourceRecords.AfsDb
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class AfsDbReader : IResourceRecordReader<AfsDbRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = AfsDbRecord.ResourceRecordType;

        public AfsDbRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new AfsDbRecord(
            info,
            (AfsType) reader.ReadUInt16NetworkOrder(),
            reader.ReadDnsName());
    }
}