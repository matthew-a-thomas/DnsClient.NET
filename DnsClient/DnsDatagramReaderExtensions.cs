namespace DnsClient
{
    using Core;
    using ResourceRecords;

    /// <summary>
    /// Extensions for <see cref="DnsDatagramReader"/>.
    /// </summary>
    public static class DnsDatagramReaderExtensions
    {
        /// <summary>
        /// Reads a <see cref="ResourceRecord"/> from this <see cref="DnsDatagramReader"/>.
        /// </summary>
        public static ResourceRecord ReadRecordInfo(this DnsDatagramReader reader)
        {
            return new ResourceRecord(
                reader.ReadQuestionQueryString(),                      // name
                (ResourceRecordType)reader.ReadUInt16NetworkOrder(),   // type
                (QueryClass)reader.ReadUInt16NetworkOrder(),           // class
                (int)reader.ReadUInt32NetworkOrder(),                  // ttl - 32bit!!
                reader.ReadUInt16NetworkOrder());                      // RDLength
        }
    }
}