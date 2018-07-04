﻿namespace DnsClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Options;

    internal class DnsRecordFactory
    {
        private readonly DnsDatagramReader _reader;

        public DnsRecordFactory(DnsDatagramReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        /*
        0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                                               |
        /                                               /
        /                      NAME                     /
        |                                               |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                      TYPE                     |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                     CLASS                     |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                      TTL                      |
        |                                               |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                   RDLENGTH                    |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--|
        /                     RDATA                     /
        /                                               /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
         * */

        public ResourceRecordInfo ReadRecordInfo()
        {
            return new ResourceRecordInfo(
                _reader.ReadQuestionQueryString(),                      // name
                (ResourceRecordType)_reader.ReadUInt16NetworkOrder(),   // type
                (QueryClass)_reader.ReadUInt16NetworkOrder(),           // class
                (int)_reader.ReadUInt32NetworkOrder(),                  // ttl - 32bit!!
                _reader.ReadUInt16NetworkOrder());                      // RDLength
        }

        public DnsResourceRecord GetRecord(ResourceRecordInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            var oldIndex = _reader.Index;
            DnsResourceRecord result;

            switch (info.RecordType)
            {
                case ResourceRecordType.A:
                    result = new ARecord(info, _reader.ReadIpAddress());
                    break;

                case ResourceRecordType.Ns:
                    result = new NsRecord(info, _reader.ReadDnsName());
                    break;

                case ResourceRecordType.Cname:
                    result = new CNameRecord(info, _reader.ReadDnsName());
                    break;

                case ResourceRecordType.Soa:
                    result = ResolveSoaRecord(info);
                    break;

                case ResourceRecordType.Mb:
                    result = new MbRecord(info, _reader.ReadDnsName());
                    break;

                case ResourceRecordType.Mg:
                    result = new MgRecord(info, _reader.ReadDnsName());
                    break;

                case ResourceRecordType.Mr:
                    result = new MrRecord(info, _reader.ReadDnsName());
                    break;

                case ResourceRecordType.Null:
                    result = new NullRecord(info, _reader.ReadBytes(info.RawDataLength).ToArray());
                    break;

                case ResourceRecordType.Wks:
                    result = ResolveWksRecord(info);
                    break;

                case ResourceRecordType.Ptr:
                    result = new PtrRecord(info, _reader.ReadDnsName());
                    break;

                case ResourceRecordType.Hinfo:
                    result = new HInfoRecord(info, _reader.ReadString(), _reader.ReadString());
                    break;

                case ResourceRecordType.Minfo:
                    result = new MInfoRecord(info, _reader.ReadDnsName(), _reader.ReadDnsName());
                    break;

                case ResourceRecordType.Mx:
                    result = ResolveMxRecord(info);
                    break;

                case ResourceRecordType.Txt:
                    result = ResolveTxtRecord(info);
                    break;

                case ResourceRecordType.Rp:
                    result = new RpRecord(info, _reader.ReadDnsName(), _reader.ReadDnsName());
                    break;

                case ResourceRecordType.Afsdb:
                    result = new AfsDbRecord(info, (AfsType)_reader.ReadUInt16NetworkOrder(), _reader.ReadDnsName());
                    break;

                case ResourceRecordType.Aaaa:
                    result = new AaaaRecord(info, _reader.ReadIPv6Address());
                    break;

                case ResourceRecordType.Srv:
                    result = ResolveSrvRecord(info);
                    break;

                case ResourceRecordType.Opt:
                    result = ResolveOptRecord(info);
                    break;

                case ResourceRecordType.Uri:
                    result = ResolveUriRecord(info);
                    break;

                case ResourceRecordType.Caa:
                    result = ResolveCaaRecord(info);
                    break;

                default:
                    // update reader index because we don't read full data for the empty record
                    _reader.Index += info.RawDataLength;
                    result = new EmptyRecord(info);
                    break;
            }

            // sanity check
            if (_reader.Index != oldIndex + info.RawDataLength)
            {
                throw new InvalidOperationException("Record reader index out of sync.");
            }

            return result;
        }

        private DnsResourceRecord ResolveUriRecord(ResourceRecordInfo info)
        {
            var prio = _reader.ReadUInt16NetworkOrder();
            var weight = _reader.ReadUInt16NetworkOrder();
            var target = _reader.ReadString(info.RawDataLength - 4);
            return new UriRecord(info, prio, weight, target);
        }

        private DnsResourceRecord ResolveOptRecord(ResourceRecordInfo info)
        {
            return new OptRecord((int)info.RecordClass, info.TimeToLive, info.RawDataLength);
        }

        private DnsResourceRecord ResolveWksRecord(ResourceRecordInfo info)
        {
            var address = _reader.ReadIpAddress();
            var protocol = _reader.ReadByte();
            var bitmap = _reader.ReadBytes(info.RawDataLength - 5);

            return new WksRecord(info, address, protocol, bitmap.ToArray());
        }

        private DnsResourceRecord ResolveMxRecord(ResourceRecordInfo info)
        {
            var preference = _reader.ReadUInt16NetworkOrder();
            var domain = _reader.ReadDnsName();

            return new MxRecord(info, preference, domain);
        }

        private DnsResourceRecord ResolveSoaRecord(ResourceRecordInfo info)
        {
            var mName = _reader.ReadDnsName();
            var rName = _reader.ReadDnsName();
            var serial = _reader.ReadUInt32NetworkOrder();
            var refresh = _reader.ReadUInt32NetworkOrder();
            var retry = _reader.ReadUInt32NetworkOrder();
            var expire = _reader.ReadUInt32NetworkOrder();
            var minimum = _reader.ReadUInt32NetworkOrder();

            return new SoaRecord(info, mName, rName, serial, refresh, retry, expire, minimum);
        }

        private DnsResourceRecord ResolveSrvRecord(ResourceRecordInfo info)
        {
            var priority = _reader.ReadUInt16NetworkOrder();
            var weight = _reader.ReadUInt16NetworkOrder();
            var port = _reader.ReadUInt16NetworkOrder();
            var target = _reader.ReadDnsName();

            return new SrvRecord(info, priority, weight, port, target);
        }

        private DnsResourceRecord ResolveTxtRecord(ResourceRecordInfo info)
        {
            var pos = _reader.Index;

            var values = new List<string>();
            var utf8Values = new List<string>();
            while (_reader.Index - pos < info.RawDataLength)
            {
                var length = _reader.ReadByte();
                var bytes = _reader.ReadBytes(length);
                var escaped = DnsDatagramReader.ParseString(bytes);
                var utf = DnsDatagramReader.ReadUtf8String(bytes);
                values.Add(escaped);
                utf8Values.Add(utf);
            }

            return new TxtRecord(info, values.ToArray(), utf8Values.ToArray());
        }

        private DnsResourceRecord ResolveCaaRecord(ResourceRecordInfo info)
        {
            var flag = _reader.ReadByte();
            var tag = _reader.ReadString();
            var stringValue = DnsDatagramReader.ParseString(_reader, info.RawDataLength - 2 - tag.Length);
            return new CaaRecord(info, flag, tag, stringValue);
        }
    }
}