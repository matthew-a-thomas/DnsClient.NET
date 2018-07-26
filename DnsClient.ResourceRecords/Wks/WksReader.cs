namespace DnsClient.ResourceRecords.Wks
{
    using System.Linq;
    using Core;

    public sealed class WksReader : IResourceRecordReader<WksRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Wks;

        public WksRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader)
        {
            var address = reader.ReadIpAddress();
            var protocol = reader.ReadByte();
            var bitmap = reader.ReadBytes(info.RawDataLength - 5);

            return new WksRecord(info, address, protocol, bitmap.ToArray());
        }
    }
}