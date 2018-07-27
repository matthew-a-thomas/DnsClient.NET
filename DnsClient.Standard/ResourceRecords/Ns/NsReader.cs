namespace DnsClient.Standard.ResourceRecords.Ns
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class NsReader : IResourceRecordReader<NsRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = NsRecord.ResourceRecordType;

        public NsRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new NsRecord(info, reader.ReadDnsName());
    }
}