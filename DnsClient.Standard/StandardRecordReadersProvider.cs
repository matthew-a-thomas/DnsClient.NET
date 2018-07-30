namespace DnsClient.Standard
{
    using System.Collections.Generic;
    using Core;
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
        private readonly Dictionary<ushort, IResourceRecordReader<DnsResourceRecord>> _dictionary
            = new Dictionary<ushort, IResourceRecordReader<DnsResourceRecord>>();

        public IReadOnlyDictionary<ushort, IResourceRecordReader<DnsResourceRecord>> RecordReaders =>
            _dictionary;

        public StandardRecordReadersProvider()
        {
            Add<AReader>();
            Add<AReader>();
            Add<AaaaReader>();
            Add<AfsDbReader>();
            Add<CaaReader>();
            Add<CNameReader>();
            Add<HInfoReader>();
            Add<MbReader>();
            Add<MgReader>();
            Add<MInfoReader>();
            Add<MrReader>();
            Add<MxReader>();
            Add<NsReader>();
            Add<NullReader>();
            Add<OptReader>();
            Add<PtrReader>();
            Add<RpReader>();
            Add<SoaReader>();
            Add<SrvReader>();
            Add<SshfpReader>();
            Add<TxtReader>();
            Add<UriReader>();
            Add<WksReader>();
        }

        private void Add<T>()
            where T : IResourceRecordReader<DnsResourceRecord>, new()
        {
            var instance = new T();
            _dictionary[instance.ResourceRecordType.Value] = instance;
        }
    }
}