﻿namespace DnsClient.Standard.ResourceRecords.Rp
{
    using Core;
    using DnsClient.ResourceRecords;

    public sealed class RpReader : IResourceRecordReader<RpRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = RpRecord.ResourceRecordType;

        public RpRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader) => new RpRecord(info, reader.ReadDnsName(), reader.ReadDnsName());
    }
}