namespace DnsClient.ResourceRecords.Null
{
    using System.Linq;
    using Core;
    using Core.ResourceRecords;

    public sealed class NullReader : IResourceRecordReader<NullRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Null;

        public NullRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new NullRecord(info, reader.ReadBytes(info.RawDataLength).ToArray());
    }
}