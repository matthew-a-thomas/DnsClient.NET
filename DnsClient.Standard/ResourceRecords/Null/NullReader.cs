﻿namespace DnsClient.Standard.ResourceRecords.Null
{
    using System.Linq;
    using Core;

    public sealed class NullReader : IResourceRecordReader<NullRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = NullRecord.ResourceRecordType;

        public NullRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new NullRecord(info, reader.ReadBytes(info.RawDataLength).ToArray());
    }
}