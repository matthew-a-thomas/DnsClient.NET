namespace DnsClient.ResourceRecords
{
    using System.Collections.Generic;
    using A;
    using Aaaa;
    using AfsDb;
    using Caa;
    using CName;
    using HInfo;
    using Mb;
    using Mg;
    using MInfo;
    using Mr;
    using Mx;
    using Ns;
    using Null;
    using Opt;
    using Ptr;
    using Rp;
    using Soa;
    using Srv;
    using Sshfp;
    using Txt;
    using Uri;
    using Wks;

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