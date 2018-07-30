namespace DnsClient.Standard.ResourceRecords.Caa
{
    using Core;

    public sealed class CaaReader : IResourceRecordReader<CaaRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = CaaRecord.ResourceRecordType;

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