namespace DnsClient.Standard.ResourceRecords.Mb
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class MbReader : IResourceRecordReader<MbRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = MbRecord.ResourceRecordType;

        public MbRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new MbRecord(info, reader.ReadDnsName());
    }
}