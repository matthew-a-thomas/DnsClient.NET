namespace DnsClient
{
    using Core;
    using Protocol;

    /// <summary>
    /// Extensions for <see cref="DnsDatagramReader"/>.
    /// </summary>
    public static class DnsDatagramReaderExtensions
    {
        /// <summary>
        /// Reads a <see cref="ResourceRecordInfo"/> from this <see cref="DnsDatagramReader"/>.
        /// </summary>
        public static ResourceRecordInfo ReadRecordInfo(this DnsDatagramReader reader)
        {
            return new ResourceRecordInfo(
                reader.ReadQuestionQueryString(),                      // name
                (ResourceRecordType)reader.ReadUInt16NetworkOrder(),   // type
                (QueryClass)reader.ReadUInt16NetworkOrder(),           // class
                (int)reader.ReadUInt32NetworkOrder(),                  // ttl - 32bit!!
                reader.ReadUInt16NetworkOrder());                      // RDLength
        }
    }
}