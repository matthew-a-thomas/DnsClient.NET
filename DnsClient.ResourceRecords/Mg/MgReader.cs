﻿namespace DnsClient.ResourceRecords.Mg
{
    using Core;

    public sealed class MgReader : IResourceRecordReader<MgRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Mg;

        public MgRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new MgRecord(info, reader.ReadDnsName());
    }
}