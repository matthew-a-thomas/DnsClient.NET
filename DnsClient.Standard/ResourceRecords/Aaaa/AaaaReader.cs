﻿namespace DnsClient.Standard.ResourceRecords.Aaaa
{
    using Core;

    public sealed class AaaaReader : IResourceRecordReader<AaaaRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = AaaaRecord.ResourceRecordType;

        public AaaaRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new AaaaRecord(info, reader.ReadIPv6Address());
    }
}