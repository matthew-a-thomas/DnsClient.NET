namespace DnsClient.Standard
{
    using System.Collections.Generic;
    using Core;
    using PseudoResourceRecords.Any;
    using PseudoResourceRecords.Axfr;
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
    using ResourceRecords.Ptr;
    using ResourceRecords.Rp;
    using ResourceRecords.RrSig;
    using ResourceRecords.Soa;
    using ResourceRecords.Srv;
    using ResourceRecords.Sshfp;
    using ResourceRecords.Txt;
    using ResourceRecords.Uri;
    using ResourceRecords.Wks;

    public sealed class StandardQueryTypesProvider
    {
        private readonly Dictionary<ushort, PseudoResourceRecordType> _dictionary =
            new Dictionary<ushort, PseudoResourceRecordType>();

        public IReadOnlyDictionary<ushort, PseudoResourceRecordType> QueryTypes => _dictionary;

        public StandardQueryTypesProvider()
        {
            Add(ARecord.ResourceRecordType);
            Add(AaaaRecord.ResourceRecordType);
            Add(AfsDbRecord.ResourceRecordType);
            Add(AnyResourceRecordType.Instance);
            Add(AxfrResourceRecordType.Instance);
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