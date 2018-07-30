namespace DnsClient.Standard.ResourceRecords.A
{
    using Core;

    public sealed class AReader : IResourceRecordReader<ARecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = ARecord.ResourceRecordType;

        public ARecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new ARecord(info, reader.ReadIpAddress());
    }
}