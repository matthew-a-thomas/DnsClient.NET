﻿namespace DnsClient.ResourceRecords.Aaaa
{
    using Core;
    using Core.ResourceRecords;

    public sealed class AaaaReader : IResourceRecordReader<AaaaRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Aaaa;

        public AaaaRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader) => new AaaaRecord(info, reader.ReadIPv6Address());
    }
}