namespace DnsClient.ResourceRecords
{
    using System;
    using Core;

    /// <summary>
    /// The type represents a <see cref="DnsResourceRecord"/>.
    /// </summary>
    public class ResourceRecord
    {
        /// <summary>
        /// The domain name used to query.
        /// </summary>
        public DnsString DomainName { get; }

        /// <summary>
        /// Specifies type of resource record.
        /// </summary>
        public PseudoResourceRecordType RecordType { get; }

        /// <summary>
        /// Specifies type class of resource record, mostly IN but can be CS, CH or HS .
        /// </summary>
        public QueryClass RecordClass { get; }

        /// <summary>
        /// The TTL value for the record set by the server.
        /// </summary>
        public int TimeToLive { get; set; }

        /// <summary>
        /// Gets the number of bytes for this resource record stored in RDATA
        /// </summary>
        public int RawDataLength { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceRecord" /> class.
        /// </summary>
        /// <param name="domainName">The <see cref="DnsString" /> used by the query.</param>
        /// <param name="recordType">Type of the record.</param>
        /// <param name="recordClass">The record class.</param>
        /// <param name="timeToLive">The time to live.</param>
        /// <param name="rawDataLength">Length of the raw data.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="domainName" /> is null or empty.</exception>
        public ResourceRecord(
            DnsString domainName,
            PseudoResourceRecordType recordType,
            QueryClass recordClass,
            int timeToLive,
            int rawDataLength)
        {
            DomainName = domainName ?? throw new ArgumentNullException(nameof(domainName));
            RecordType = recordType;
            RecordClass = recordClass;
            TimeToLive = timeToLive;
            RawDataLength = rawDataLength;
        }
    }
}