﻿namespace DnsClient.Standard.ResourceRecords.Mb
{
    using System;
    using Core;
    using DnsClient.ResourceRecords;

    /*
    https://tools.ietf.org/html/rfc1035#section-3.3.3:
    3.3.3. MB RDATA format (EXPERIMENTAL)

        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        /                   MADNAME                     /
        /                                               /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

    where:

    MADNAME         A <domain-name> which specifies a host which has the
                    specified mailbox.

     */

    /// <summary>
    /// A <see cref="DnsResourceRecord"/> represending a domain name which specifies a host which has the specified mailbox.
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.3">RFC 1035</seealso>
    public class MbRecord : DnsResourceRecord
    {
        /// <summary>
        /// A mailbox domain name (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.3">RFC 1035</seealso>
        /// <seealso cref="MbRecord"/>
        public static readonly PseudoResourceRecordType ResourceRecordType = new PseudoResourceRecordType(
            abbreviation: "Mb",
            value: 7
        );

        /// <summary>
        /// Gets the domain name which specifies a host which has the specified mailbox.
        /// </summary>
        /// <value>
        /// The domain name.
        /// </value>
        public DnsString MadName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MbRecord"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="domainName">The domain name.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="domainName"/> or <paramref name="info"/> is null.</exception>
        public MbRecord(ResourceRecord info, DnsString domainName)
            : base(info)
        {
            MadName = domainName ?? throw new ArgumentNullException(nameof(domainName));
        }

        protected override string RecordToString()
        {
            return MadName.Value;
        }
    }
}