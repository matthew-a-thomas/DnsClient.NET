namespace DnsClient
{
    using ResourceRecords;

    public sealed class StandardDnsRecordFactoryFactory
    {
        private readonly StandardRecordReadersProvider _recordReadersProvider = new StandardRecordReadersProvider();

        public DnsRecordFactory Create() => new DnsRecordFactory(_recordReadersProvider.RecordReaders);
    }
}