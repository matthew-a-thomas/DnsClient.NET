﻿namespace DnsClient.Standard.ResourceRecords.AfsDb
{
    using System;
    using Core;
    using DnsClient.ResourceRecords;

    /* https://tools.ietf.org/html/rfc1183#section-1, https://tools.ietf.org/html/rfc5864
    1. AFS Data Base location

       This section defines an extension of the DNS to locate servers both
       for AFS (AFS is a registered trademark of Transarc Corporation) and
       for the Open Software Foundation's (OSF) Distributed Computing
       Environment (DCE) authenticated naming system using HP/Apollo's NCA,
       both to be components of the OSF DCE.  The discussion assumes that
       the reader is familiar with AFS [5] and NCA [6].

       The AFS (originally the Andrew File System) system uses the DNS to
       map from a domain name to the name of an AFS cell database server.
       The DCE Naming service uses the DNS for a similar function: mapping
       from the domain name of a cell to authenticated name servers for that
       cell.  The method uses a new RR type with mnemonic AFSDB and type
       code of 18 (decimal).

       AFSDB has the following format:

       <owner> <ttl> <class> AFSDB <subtype> <hostname>
    */

    /// <summary>
    /// A <see cref="DnsResourceRecord"/> representing an AFS database location.
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc1183#section-1">RFC 1183</seealso>
    /// <seealso href="https://tools.ietf.org/html/rfc5864">RFC 5864</seealso>
    public class AfsDbRecord : DnsResourceRecord
    {
        /// <summary>
        /// AFS Data Base location.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1183#section-1">RFC 1183</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc5864">RFC 5864</seealso>
        /// <seealso cref="AfsDbRecord"/>
        public static readonly PseudoResourceRecordType ResourceRecordType = new PseudoResourceRecordType(
            abbreviation: "AfsDb",
            value: 18
        );

        /// <summary>
        /// Gets the <see cref="AfsType"/>.
        /// </summary>
        /// <value>
        /// The sub type.
        /// </value>
        public AfsType SubType { get; }

        /// <summary>
        /// Gets the hostname.
        /// </summary>
        /// <value>
        /// The hostname.
        /// </value>
        public DnsString Hostname { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfsDbRecord"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="info"/> or <paramref name="name"/> is null.</exception>
        public AfsDbRecord(ResourceRecord info, AfsType type, DnsString name) : base(info)
        {
            SubType = type;
            Hostname = name ?? throw new ArgumentNullException(nameof(name));
        }

        protected override string RecordToString()
        {
            return $"{(int)SubType} {Hostname}";
        }
    }
}