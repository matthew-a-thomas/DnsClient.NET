namespace DnsClient.Standard.ResourceRecords.HInfo
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class HInfoReader : IResourceRecordReader<HInfoRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Hinfo;

        public HInfoRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new HInfoRecord(info, reader.ReadString(), reader.ReadString());
    }
}