namespace DnsClient
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Core.ResourceRecords;
    using ResourceRecords;
    using ResourceRecords.A;
    using ResourceRecords.Aaaa;
    using ResourceRecords.AfsDb;
    using ResourceRecords.Caa;
    using ResourceRecords.CName;
    using ResourceRecords.HInfo;
    using ResourceRecords.Mb;
    using ResourceRecords.Mg;
    using ResourceRecords.MInfo;
    using ResourceRecords.Mr;
    using ResourceRecords.Mx;
    using ResourceRecords.Ns;
    using ResourceRecords.Null;
    using ResourceRecords.Opt;
    using ResourceRecords.Ptr;
    using ResourceRecords.Rp;
    using ResourceRecords.Soa;
    using ResourceRecords.Srv;
    using ResourceRecords.Sshfp;
    using ResourceRecords.Txt;
    using ResourceRecords.Uri;
    using ResourceRecords.Wks;

    internal class DnsRecordFactory
    {
        private readonly DnsDatagramReader _reader;
        private readonly IReadOnlyDictionary<ResourceRecordType, IResourceRecordReader<DnsResourceRecord>>
            _recordReaders = new Dictionary<ResourceRecordType, IResourceRecordReader<DnsResourceRecord>>
            {
                { ResourceRecordType.A, new AReader() },
                { ResourceRecordType.Aaaa, new AaaaReader() },
                { ResourceRecordType.AfsDb, new AfsDbReader() },
                { ResourceRecordType.Caa, new CaaReader() },
                { ResourceRecordType.Cname, new CNameReader() },
                { ResourceRecordType.Hinfo, new HInfoReader() },
                { ResourceRecordType.Mb, new MbReader() },
                { ResourceRecordType.Mg, new MgReader() },
                { ResourceRecordType.Minfo, new MInfoReader() },
                { ResourceRecordType.Mr, new MrReader() },
                { ResourceRecordType.Mx, new MxReader() },
                { ResourceRecordType.Ns, new NsReader() },
                { ResourceRecordType.Null, new NullReader() },
                { ResourceRecordType.Opt, new OptReader() },
                { ResourceRecordType.Ptr, new PtrReader() },
                { ResourceRecordType.Rp, new RpReader() },
                { ResourceRecordType.Soa, new SoaReader() },
                { ResourceRecordType.Srv, new SrvReader() },
                { ResourceRecordType.Sshfp, new SshfpReader() },
                { ResourceRecordType.Txt, new TxtReader() },
                { ResourceRecordType.Uri, new UriReader() },
                { ResourceRecordType.Wks, new WksReader() }
            };

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

        public DnsResourceRecord GetRecord(ResourceRecordInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            var oldIndex = _reader.Index;
            DnsResourceRecord result;

            if (_recordReaders.TryGetValue(info.RecordType, out var recordReader))
            {
                result = recordReader.ReadResourceRecord(info, _reader);
            }
            else
            {
                // update reader index because we don't read full data for the empty record
                _reader.Index += info.RawDataLength;
                result = new EmptyRecord(info);
            }

            // sanity check
            if (_reader.Index != oldIndex + info.RawDataLength)
            {
                throw new InvalidOperationException("Record reader index out of sync.");
            }

            return result;
        }
    }
}