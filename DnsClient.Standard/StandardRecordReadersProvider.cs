namespace DnsClient.Standard
{
    using System.Collections.Generic;
    using DnsClient.ResourceRecords;
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

    public sealed class StandardRecordReadersProvider
    {
        public IReadOnlyDictionary<ResourceRecordType, IResourceRecordReader<DnsResourceRecord>> RecordReaders { get; }
            = new Dictionary<ResourceRecordType, IResourceRecordReader<DnsResourceRecord>>
            {
                {ResourceRecordType.A, new AReader()},
                {ResourceRecordType.Aaaa, new AaaaReader()},
                {ResourceRecordType.AfsDb, new AfsDbReader()},
                {ResourceRecordType.Caa, new CaaReader()},
                {ResourceRecordType.Cname, new CNameReader()},
                {ResourceRecordType.Hinfo, new HInfoReader()},
                {ResourceRecordType.Mb, new MbReader()},
                {ResourceRecordType.Mg, new MgReader()},
                {ResourceRecordType.Minfo, new MInfoReader()},
                {ResourceRecordType.Mr, new MrReader()},
                {ResourceRecordType.Mx, new MxReader()},
                {ResourceRecordType.Ns, new NsReader()},
                {ResourceRecordType.Null, new NullReader()},
                {ResourceRecordType.Opt, new OptReader()},
                {ResourceRecordType.Ptr, new PtrReader()},
                {ResourceRecordType.Rp, new RpReader()},
                {ResourceRecordType.Soa, new SoaReader()},
                {ResourceRecordType.Srv, new SrvReader()},
                {ResourceRecordType.Sshfp, new SshfpReader()},
                {ResourceRecordType.Txt, new TxtReader()},
                {ResourceRecordType.Uri, new UriReader()},
                {ResourceRecordType.Wks, new WksReader()}
            };
    }
}