namespace DnsClient.Standard.ResourceRecords.Opt
{
    using Core;

    public sealed class OptReader : IResourceRecordReader<OptRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = OptRecord.ResourceRecordType;

        public OptRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new OptRecord((int)info.RecordClass, info.TimeToLive, info.RawDataLength);
    }
}