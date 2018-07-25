namespace DnsClient.Core.Protocol
{
    using System;

    /*
     * RFC 1035 (https://tools.ietf.org/html/rfc1035#section-3.2.2)
     * */

    /// <summary>
    /// The resource record types. The <c>enum</c> contains only the types supported by this library at this moment.
    /// The <see cref="ResourceRecordType"/> is used to identify any <see cref="DnsResourceRecord"/>.
    /// <para>
    /// Resource record types are a subset of <see cref="QueryType"/>.
    /// </para>
    /// </summary>
    /// <seealso cref="DnsResourceRecord"/>
    /// <seealso cref="ResourceRecordType"/>
    public enum ResourceRecordType : short
    {
        /// <summary>
        /// A host address.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        /// <seealso cref="ARecord"/>
        A = 1,

        /// <summary>
        /// An authoritative name server.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso cref="NsRecord"/>
        Ns = 2,

        /// <summary>
        /// A mail destination (OBSOLETE - use MX).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        [Obsolete("Use MX")]
        Md = 3,

        /// <summary>
        /// A mail forwarder (OBSOLETE - use MX).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        [Obsolete("Use MX")]
        Mf = 4,

        /// <summary>
        /// The canonical name for an alias.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.1">RFC 1035</seealso>
        /// <seealso cref="CNameRecord"/>
        Cname = 5,

        /// <summary>
        /// Marks the start of a zone of authority.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.13">RFC 1035</seealso>
        /// <seealso cref="SoaRecord"/>
        Soa = 6,

        /// <summary>
        /// A mailbox domain name (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.3">RFC 1035</seealso>
        /// <seealso cref="MbRecord"/>
        Mb = 7,

        /// <summary>
        /// A mail group member (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.6">RFC 1035</seealso>
        /// <seealso cref="MgRecord"/>
        Mg = 8,

        /// <summary>
        /// A mailbox rename domain name (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.8">RFC 1035</seealso>
        /// <seealso cref="MrRecord"/>
        Mr = 9,

        /// <summary>
        /// A Null resource record (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.8">RFC 1035</seealso>
        /// <seealso cref="NullRecord"/>
        Null = 10,

        /// <summary>
        /// A well known service description.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3232">RFC 3232</seealso>
        /// <seealso cref="WksRecord"/>
        Wks = 11,

        /// <summary>
        /// A domain name pointer.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.12">RFC 1035</seealso>
        /// <seealso cref="PtrRecord"/>
        Ptr = 12,

        /// <summary>
        /// Host information.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc1010">RFC 1010</seealso>
        /// <seealso cref="HInfoRecord"/>
        Hinfo = 13,

        /// <summary>
        /// Mailbox or mail list information.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso cref="MInfoRecord"/>
        Minfo = 14,

        /// <summary>
        /// Mail exchange.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.9">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc974">RFC 974</seealso>
        /// <seealso cref="MxRecord"/>
        Mx = 15,

        /// <summary>
        /// Text resources.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc1464">RFC 1464</seealso>
        /// <seealso cref="TxtRecord"/>
        Txt = 16,

        /// <summary>
        /// Responsible Person.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1183">RFC 1183</seealso>
        /// <seealso cref="RpRecord"/>
        Rp = 17,

        /// <summary>
        /// AFS Data Base location.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1183#section-1">RFC 1183</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc5864">RFC 5864</seealso>
        /// <seealso cref="AfsDbRecord"/>
        Afsdb = 18,

        /// <summary>
        /// An IPv6 host address.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3596#section-2.2">RFC 3596</seealso>
        /// <seealso cref="AaaaRecord"/>
        Aaaa = 28,

        /// <summary>
        /// A resource record which specifies the location of the server(s) for a specific protocol and domain.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc2782">RFC 2782</seealso>
        /// <seealso cref="SrvRecord"/>
        Srv = 33,

        /// <summary>
        /// Option record.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc6891">RFC 6891</seealso>
        ///// <seealso cref="DnsClient.Protocol.Options.OptRecord"/>
        Opt = 41,

        /// <summary>
        /// SSH finger print record
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc4255">RFC 4255</seealso>
        Sshfp = 44,

        /// <summary>
        /// RRSIG rfc3755.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3755">RFC 3755</seealso>
        Rrsig = 46,

        /// <summary>
        /// A Uniform Resource Identifier (URI) resource record.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc7553">RFC 7553</seealso>
        /// <seealso cref="UriRecord"/>
        Uri = 256,

        /// <summary>
        /// A certification authority authorization.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc6844">RFC 6844</seealso>
        /// <seealso cref="CaaRecord"/>
        Caa = 257
    }
}