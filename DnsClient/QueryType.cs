namespace DnsClient
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using ResourceRecords;
    using Standard.ResourceRecords.A;
    using Standard.ResourceRecords.Aaaa;
    using Standard.ResourceRecords.AfsDb;
    using Standard.ResourceRecords.Caa;
    using Standard.ResourceRecords.CName;
    using Standard.ResourceRecords.HInfo;
    using Standard.ResourceRecords.Mb;
    using Standard.ResourceRecords.Mg;
    using Standard.ResourceRecords.MInfo;
    using Standard.ResourceRecords.Mr;
    using Standard.ResourceRecords.Mx;
    using Standard.ResourceRecords.Ns;
    using Standard.ResourceRecords.Null;
    using Standard.ResourceRecords.Ptr;
    using Standard.ResourceRecords.Rp;
    using Standard.ResourceRecords.Soa;
    using Standard.ResourceRecords.Srv;
    using Standard.ResourceRecords.Sshfp;
    using Standard.ResourceRecords.Txt;
    using Standard.ResourceRecords.Uri;
    using Standard.ResourceRecords.Wks;

    /*
     * RFC 1035 (https://tools.ietf.org/html/rfc1035#section-3.2.3)
     * */

    /// <summary>
    /// The query type field appear in the question part of a query.
    /// Query types are a superset of <see cref="PseudoResourceRecordType"/>.
    /// </summary>
    [SuppressMessage("ReSharper",
        "UnusedMember.Global")]
    public enum QueryType : short
    {
        /// <summary>
        /// A host address.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        /// <seealso cref="ARecord"/>
        A = PseudoResourceRecordType.A,

        /// <summary>
        /// An authoritative name server.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso cref="NsRecord"/>
        Ns = PseudoResourceRecordType.Ns,

        /// <summary>
        /// A mail destination (OBSOLETE - use MX).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        [Obsolete("Use MX")]
        Md = PseudoResourceRecordType.Md,

        /// <summary>
        /// A mail forwarder (OBSOLETE - use MX).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        [Obsolete("Use MX")]
        Mf = PseudoResourceRecordType.Mf,

        /// <summary>
        /// The canonical name for an alias.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.1">RFC 1035</seealso>
        /// <seealso cref="CNameRecord"/>
        Cname = PseudoResourceRecordType.Cname,

        /// <summary>
        /// Marks the start of a zone of authority.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.13">RFC 1035</seealso>
        /// <seealso cref="SoaRecord"/>
        Soa = PseudoResourceRecordType.Soa,

        /// <summary>
        /// A mailbox domain name (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.3">RFC 1035</seealso>
        /// <seealso cref="MbRecord"/>
        Mb = PseudoResourceRecordType.Mb,

        /// <summary>
        /// A mail group member (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.6">RFC 1035</seealso>
        /// <seealso cref="MgRecord"/>
        Mg = PseudoResourceRecordType.Mg,

        /// <summary>
        /// A mailbox rename domain name (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.8">RFC 1035</seealso>
        /// <seealso cref="MrRecord"/>
        Mr = PseudoResourceRecordType.Mr,

        /// <summary>
        /// A Null resource record (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.8">RFC 1035</seealso>
        /// <seealso cref="NullRecord"/>
        Null = PseudoResourceRecordType.Null,

        /// <summary>
        /// A well known service description.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3232">RFC 3232</seealso>
        /// <seealso cref="WksRecord"/>
        Wks = PseudoResourceRecordType.Wks,

        /// <summary>
        /// A domain name pointer.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.12">RFC 1035</seealso>
        /// <seealso cref="PtrRecord"/>
        Ptr = PseudoResourceRecordType.Ptr,

        /// <summary>
        /// Host information.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc1010">RFC 1010</seealso>
        /// <seealso cref="HInfoRecord"/>
        Hinfo = PseudoResourceRecordType.Hinfo,

        /// <summary>
        /// Mailbox or mail list information.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso cref="MInfoRecord"/>
        Minfo = PseudoResourceRecordType.Minfo,

        /// <summary>
        /// Mail exchange.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.9">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc974">RFC 974</seealso>
        /// <seealso cref="MxRecord"/>
        Mx = PseudoResourceRecordType.Mx,

        /// <summary>
        /// Text resources.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc1464">RFC 1464</seealso>
        /// <seealso cref="TxtRecord"/>
        Txt = PseudoResourceRecordType.Txt,

        /// <summary>
        /// Responsible Person.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1183">RFC 1183</seealso>
        /// <seealso cref="RpRecord"/>
        Rp = PseudoResourceRecordType.Rp,

        /// <summary>
        /// AFS Data Base location.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1183#section-1">RFC 1183</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc5864">RFC 5864</seealso>
        /// <seealso cref="AfsDbRecord"/>
        Afsdb = PseudoResourceRecordType.AfsDb,

        /// <summary>
        /// An IPv6 host address.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3596#section-2.2">RFC 3596</seealso>
        /// <seealso cref="AaaaRecord"/>
        Aaaa = PseudoResourceRecordType.Aaaa,

        /// <summary>
        /// A resource record which specifies the location of the server(s) for a specific protocol and domain.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc2782">RFC 2782</seealso>
        /// <seealso cref="SrvRecord"/>
        Srv = PseudoResourceRecordType.Srv,

        /// <summary>
        /// RRSIG rfc3755.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3755">RFC 3755</seealso>
        Rrsig = PseudoResourceRecordType.Rrsig,

        /// <summary>
        /// DNS zone transfer request.
        /// This can be used only if <see cref="ILookupClient.UseTcpOnly"/> is set to true as <c>AXFR</c> is only supported via TCP.
        /// <para>
        /// The DNS Server might only return results for the request if the client connection/IP is allowed to do so.
        /// </para>
        /// </summary>
        Axfr = 252,

        /// <summary>
        /// Generic any query *.
        /// </summary>
        Any = 255,

        /// <summary>
        /// A Uniform Resource Identifier (URI) resource record.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc7553">RFC 7553</seealso>
        /// <seealso cref="UriRecord"/>
        Uri = PseudoResourceRecordType.Uri,

        /// <summary>
        /// A certification authority authorization.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc6844">RFC 6844</seealso>
        /// <seealso cref="CaaRecord"/>
        Caa = PseudoResourceRecordType.Caa,

        /// <summary>
        /// A SSH Fingerprint resource record.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc4255">RFC 4255</seealso>
        /// <seealso cref="SshfpRecord"/>
        Sshfp = PseudoResourceRecordType.Sshfp
    }
}