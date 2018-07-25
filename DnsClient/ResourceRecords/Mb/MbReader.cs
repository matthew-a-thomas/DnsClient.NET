namespace DnsClient.ResourceRecords.Mb
{
    using Core;
    using Core.Protocol;

    public sealed class MbReader : IResourceRecordReader<MbRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Mb;

        public MbRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new MbRecord(info, reader.ReadDnsName());
    }
}