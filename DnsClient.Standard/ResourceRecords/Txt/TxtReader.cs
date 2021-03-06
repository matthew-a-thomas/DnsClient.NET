﻿namespace DnsClient.Standard.ResourceRecords.Txt
{
    using System.Collections.Generic;
    using Core;

    public sealed class TxtReader : IResourceRecordReader<TxtRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = TxtRecord.ResourceRecordType;

        public TxtRecord ReadResourceRecord(
            ResourceRecord info,
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