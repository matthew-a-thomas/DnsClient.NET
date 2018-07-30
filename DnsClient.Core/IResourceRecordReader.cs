namespace DnsClient.Core
{
    /// <summary>
    /// Something which knows how to get a resource record from a <see cref="DnsDatagramReader"/>.
    /// </summary>
    public interface IResourceRecordReader<out T>
    {
        /// <summary>
        /// The type of record that this <see cref="IResourceRecordReader{T}"/> knows how to get.
        /// </summary>
        PseudoResourceRecordType ResourceRecordType { get; }

        /// <summary>
        /// Reads a resource record from the given <see cref="DnsDatagramReader"/> using the given
        /// <paramref name="info"/>.
        /// </summary>
        T ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader);
    }
}