﻿namespace DnsClient.ResourceRecords.Caa
{
    using Core;
    using Core.Protocol;
    public sealed class CaaReader : IResourceRecordReader<CaaRecord>
    {
        public ResourceRecordType ResourceRecordType { get; } = ResourceRecordType.Caa;

        public CaaRecord ReadResourceRecord(
            ResourceRecordInfo info,
            DnsDatagramReader reader)
        {
            var flag = reader.ReadByte();
            var tag = reader.ReadString();
            var stringValue = DnsDatagramReader.ParseString(reader, info.RawDataLength - 2 - tag.Length);
            return new CaaRecord(info, flag, tag, stringValue);
        }
    }
}