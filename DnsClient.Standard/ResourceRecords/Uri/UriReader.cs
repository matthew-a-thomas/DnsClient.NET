namespace DnsClient.Standard.ResourceRecords.Uri
{
    using Core;

    public sealed class UriReader : IResourceRecordReader<UriRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = UriRecord.ResourceRecordType;

        public UriRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader)
        {
            var prio = reader.ReadUInt16NetworkOrder();
            var weight = reader.ReadUInt16NetworkOrder();
            var target = reader.ReadString(info.RawDataLength - 4);
            return new UriRecord(info, prio, weight, target);
        }
    }
}