namespace DnsClient.ResourceRecords.Txt
{
    using System.Collections.Generic;
    using Core;
    using Core.ResourceRecords;

    public sealed class TxtReader : IResourceRecordReader<TxtRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Txt;

        public TxtRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader)
        {
            var pos = reader.Index;

            var values = new List<string>();
            var utf8Values = new List<string>();
            while (reader.Index - pos < info.RawDataLength)
            {
                var length = reader.ReadByte();
                var bytes = reader.ReadBytes(length);
                var escaped = DnsDatagramReader.ParseString(bytes);
                var utf = DnsDatagramReader.ReadUtf8String(bytes);
                values.Add(escaped);
                utf8Values.Add(utf);
            }

            return new TxtRecord(info, values.ToArray(), utf8Values.ToArray());
        }
    }
}