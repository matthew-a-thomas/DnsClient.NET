namespace DnsClient.Standard
{
    using System.Collections.Generic;
    using Core;
    using ResourceRecords.A;
    using ResourceRecords.Aaaa;
    using ResourceRecords.AfsDb;
    using ResourceRecords.Caa;
    using ResourceRecords.CName;
    using ResourceRecords.HInfo;
    using ResourceRecords.Mb;
    using ResourceRecords.Md;
    using ResourceRecords.Mf;
    using ResourceRecords.Mg;
    using ResourceRecords.MInfo;
    using ResourceRecords.Mr;
    using ResourceRecords.Mx;
    using ResourceRecords.Ns;
    using ResourceRecords.Null;
    using ResourceRecords.Opt;
    using ResourceRecords.Ptr;
    using ResourceRecords.Rp;
    using ResourceRecords.RrSig;
    using ResourceRecords.Soa;
    using ResourceRecords.Srv;
    using ResourceRecords.Sshfp;
    using ResourceRecords.Txt;
    using ResourceRecords.Uri;
    using ResourceRecords.Wks;

    public sealed class StandardResourceRecordTypesProvider
    {
        private readonly Dictionary<ushort, PseudoResourceRecordType> _dictionary =
            new Dictionary<ushort, PseudoResourceRecordType>();

        public IReadOnlyDictionary<ushort, PseudoResourceRecordType> ResourceRecordTypes => _dictionary;

        public StandardResourceRecordTypesProvider()
        {
            Add(ARecord.ResourceRecordType);
            Add(AaaaRecord.ResourceRecordType);
            Add(AfsDbRecord.ResourceRecordType);
            Add(CaaRecord.ResourceRecordType);
            Add(CNameRecord.ResourceRecordType);
            Add(HInfoRecord.ResourceRecordType);
            Add(MbRecord.ResourceRecordType);
#pragma warning disable 618
            Add(MdResourceRecordType.Instance);
            Add(MfResourceRecordType.Instance);
#pragma warning restore 618
            Add(MgRecord.ResourceRecordType);
            Add(MInfoRecord.ResourceRecordType);
            Add(MrRecord.ResourceRecordType);
            Add(MxRecord.ResourceRecordType);
            Add(NsRecord.ResourceRecordType);
            Add(NullRecord.ResourceRecordType);
            Add(OptRecord.ResourceRecordType);
            Add(PtrRecord.ResourceRecordType);
            Add(RpRecord.ResourceRecordType);
            Add(RrSigResourceRecordType.Instance);
            Add(SoaRecord.ResourceRecordType);
            Add(SrvRecord.ResourceRecordType);
            Add(SshfpRecord.ResourceRecordType);
            Add(TxtRecord.ResourceRecordType);
            Add(UriRecord.ResourceRecordType);
            Add(WksRecord.ResourceRecordType);
        }

        private void Add(PseudoResourceRecordType type) => _dictionary[type.Value] = type;
    }
}