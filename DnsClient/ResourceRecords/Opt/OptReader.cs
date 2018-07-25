namespace DnsClient.ResourceRecords.Opt
{
    using Core;
    using Core.Protocol;
    public sealed class OptReader : IResourceRecordReader<OptRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Opt;

        public OptRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new OptRecord((int)info.RecordClass, info.TimeToLive, info.RawDataLength);
    }
}