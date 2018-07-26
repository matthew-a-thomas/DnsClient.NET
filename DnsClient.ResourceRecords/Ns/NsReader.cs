namespace DnsClient.ResourceRecords.Ns
{
    using Core;

    public sealed class NsReader : IResourceRecordReader<NsRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Ns;

        public NsRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new NsRecord(info, reader.ReadDnsName());
    }
}