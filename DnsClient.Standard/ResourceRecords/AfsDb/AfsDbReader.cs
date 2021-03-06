﻿namespace DnsClient.Standard.ResourceRecords.AfsDb
{
    using Core;

    public sealed class AfsDbReader : IResourceRecordReader<AfsDbRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = AfsDbRecord.ResourceRecordType;

        public AfsDbRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new AfsDbRecord(
            info,
            (AfsType) reader.ReadUInt16NetworkOrder(),
            reader.ReadDnsName());
    }
}