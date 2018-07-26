namespace DnsClient.Standard.ResourceRecords.Null
{
    using System.Linq;
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class NullReader : IResourceRecordReader<NullRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Null;

        public NullRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new NullRecord(info, reader.ReadBytes(info.RawDataLength).ToArray());
    }
}