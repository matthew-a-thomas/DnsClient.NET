﻿namespace DnsClient.ResourceRecords
{
    using Core;

    /// <summary>
    /// Something which knows how to get a <see cref="DnsResourceRecord"/> from a <see cref="DnsDatagramReader"/>.
    /// </summary>
    public interface IResourceRecordReader<out T>
    {
        /// <summary>
        /// The type of <see cref="DnsResourceRecord"/> that this <see cref="IResourceRecordReader{T}"/> knows how to
        /// get.
        /// </summary>
        PseudoResourceRecordType ResourceRecordType { get; }

        /// <summary>
        /// Reads a <see cref="DnsResourceRecord"/> from the given <see cref="DnsDatagramReader"/> using the given
        /// <paramref name="info"/>.
        /// </summary>
        T ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader);
    }
}