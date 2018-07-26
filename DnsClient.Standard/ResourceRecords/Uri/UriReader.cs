namespace DnsClient.Standard.ResourceRecords.Uri
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class UriReader : IResourceRecordReader<UriRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Uri;

        public UriRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader)
        {
            var prio = reader.ReadUInt16NetworkOrder();
            var weight = reader.ReadUInt16NetworkOrder();
            var target = reader.ReadString(info.RawDataLength - 4);
            return new UriRecord(info, prio, weight, target);
        }
    }
}