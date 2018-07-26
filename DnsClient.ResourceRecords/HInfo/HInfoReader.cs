namespace DnsClient.ResourceRecords.HInfo
{
    using Core;

    public sealed class HInfoReader : IResourceRecordReader<HInfoRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Hinfo;

        public HInfoRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new HInfoRecord(info, reader.ReadString(), reader.ReadString());
    }
}