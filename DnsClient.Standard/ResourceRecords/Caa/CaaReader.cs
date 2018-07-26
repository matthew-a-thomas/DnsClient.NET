namespace DnsClient.Standard.ResourceRecords.Caa
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class CaaReader : IResourceRecordReader<CaaRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Caa;

        public CaaRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader)
        {
            var flag = reader.ReadByte();
            var tag = reader.ReadString();
            var stringValue = DnsDatagramReader.ParseString(reader, info.RawDataLength - 2 - tag.Length);
            return new CaaRecord(info, flag, tag, stringValue);
        }
    }
}