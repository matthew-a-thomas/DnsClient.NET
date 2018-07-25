namespace DnsClient
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Core.Protocol;
    using ResourceRecords.A;
    using ResourceRecords.Aaaa;
    using ResourceRecords.AfsDb;
    using ResourceRecords.Caa;
    using ResourceRecords.CName;
    using ResourceRecords.HInfo;
    using ResourceRecords.Mb;
    using ResourceRecords.Mg;
    using ResourceRecords.MInfo;
    using ResourceRecords.Mr;
    using ResourceRecords.Mx;
    using ResourceRecords.Ns;
    using ResourceRecords.Null;
    using ResourceRecords.Ptr;
    using ResourceRecords.Rp;
    using ResourceRecords.Soa;
    using ResourceRecords.Srv;
    using ResourceRecords.Sshfp;
    using ResourceRecords.Txt;
    using ResourceRecords.Uri;
    using ResourceRecords.Wks;

    /*
     * RFC 1035 (https://tools.ietf.org/html/rfc1035#section-3.2.3)
     * */

    /// <summary>
    /// The query type field appear in the question part of a query.
    /// Query types are a superset of <see cref="ResourceRecordType"/>.
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
        A = ResourceRecordType.A,

        /// <summary>
        /// An authoritative name server.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso cref="NsRecord"/>
        Ns = ResourceRecordType.Ns,

        /// <summary>
        /// A mail destination (OBSOLETE - use MX).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        [Obsolete("Use MX")]
        Md = ResourceRecordType.Md,

        /// <summary>
        /// A mail forwarder (OBSOLETE - use MX).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        [Obsolete("Use MX")]
        Mf = ResourceRecordType.Mf,

        /// <summary>
        /// The canonical name for an alias.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.1">RFC 1035</seealso>
        /// <seealso cref="CNameRecord"/>
        Cname = ResourceRecordType.Cname,

        /// <summary>
        /// Marks the start of a zone of authority.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.13">RFC 1035</seealso>
        /// <seealso cref="SoaRecord"/>
        Soa = ResourceRecordType.Soa,

        /// <summary>
        /// A mailbox domain name (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.3">RFC 1035</seealso>
        /// <seealso cref="MbRecord"/>
        Mb = ResourceRecordType.Mb,

        /// <summary>
        /// A mail group member (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.6">RFC 1035</seealso>
        /// <seealso cref="MgRecord"/>
        Mg = ResourceRecordType.Mg,

        /// <summary>
        /// A mailbox rename domain name (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.8">RFC 1035</seealso>
        /// <seealso cref="MrRecord"/>
        Mr = ResourceRecordType.Mr,

        /// <summary>
        /// A Null resource record (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.8">RFC 1035</seealso>
        /// <seealso cref="NullRecord"/>
        Null = ResourceRecordType.Null,

        /// <summary>
        /// A well known service description.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3232">RFC 3232</seealso>
        /// <seealso cref="WksRecord"/>
        Wks = ResourceRecordType.Wks,

        /// <summary>
        /// A domain name pointer.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.12">RFC 1035</seealso>
        /// <seealso cref="PtrRecord"/>
        Ptr = ResourceRecordType.Ptr,

        /// <summary>
        /// Host information.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc1010">RFC 1010</seealso>
        /// <seealso cref="HInfoRecord"/>
        Hinfo = ResourceRecordType.Hinfo,

        /// <summary>
        /// Mailbox or mail list information.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso cref="MInfoRecord"/>
        Minfo = ResourceRecordType.Minfo,

        /// <summary>
        /// Mail exchange.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.9">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc974">RFC 974</seealso>
        /// <seealso cref="MxRecord"/>
        Mx = ResourceRecordType.Mx,

        /// <summary>
        /// Text resources.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc1464">RFC 1464</seealso>
        /// <seealso cref="TxtRecord"/>
        Txt = ResourceRecordType.Txt,

        /// <summary>
        /// Responsible Person.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1183">RFC 1183</seealso>
        /// <seealso cref="RpRecord"/>
        Rp = ResourceRecordType.Rp,

        /// <summary>
        /// AFS Data Base location.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1183#section-1">RFC 1183</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc5864">RFC 5864</seealso>
        /// <seealso cref="AfsDbRecord"/>
        Afsdb = ResourceRecordType.AfsDb,

        /// <summary>
        /// An IPv6 host address.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3596#section-2.2">RFC 3596</seealso>
        /// <seealso cref="AaaaRecord"/>
        Aaaa = ResourceRecordType.Aaaa,

        /// <summary>
        /// A resource record which specifies the location of the server(s) for a specific protocol and domain.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc2782">RFC 2782</seealso>
        /// <seealso cref="SrvRecord"/>
        Srv = ResourceRecordType.Srv,

        /// <summary>
        /// RRSIG rfc3755.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3755">RFC 3755</seealso>
        Rrsig = ResourceRecordType.Rrsig,

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
        Uri = ResourceRecordType.Uri,

        /// <summary>
        /// A certification authority authorization.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc6844">RFC 6844</seealso>
        /// <seealso cref="CaaRecord"/>
        Caa = ResourceRecordType.Caa,

        /// <summary>
        /// A SSH Fingerprint resource record.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc4255">RFC 4255</seealso>
        /// <seealso cref="SshfpRecord"/>
        Sshfp = ResourceRecordType.Sshfp
    }
}