namespace DnsClient.Standard.ResourceRecords.Wks
{
    using System.Linq;
    using Core;

    public sealed class WksReader : IResourceRecordReader<WksRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = WksRecord.ResourceRecordType;

        public WksRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader)
        {
            var address = reader.ReadIpAddress();
            var protocol = reader.ReadByte();
            var bitmap = reader.ReadBytes(info.RawDataLength - 5);

            return new WksRecord(info, address, protocol, bitmap.ToArray());
        }
    }
}