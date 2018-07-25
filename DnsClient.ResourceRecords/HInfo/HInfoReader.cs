namespace DnsClient.ResourceRecords.HInfo
{
    using Core;
    using Core.ResourceRecords;

    public sealed class HInfoReader : IResourceRecordReader<HInfoRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Hinfo;

        public HInfoRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new HInfoRecord(info, reader.ReadString(), reader.ReadString());
    }
}