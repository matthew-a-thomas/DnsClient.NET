namespace DnsClient.Standard.ResourceRecords.Mb
{
    using Core;

    public sealed class MbReader : IResourceRecordReader<MbRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = MbRecord.ResourceRecordType;

        public MbRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new MbRecord(info, reader.ReadDnsName());
    }
}