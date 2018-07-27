﻿namespace DnsClient.Standard.ResourceRecords.Md
{
    using System;
    using DnsClient.ResourceRecords;

    public static class MdResourceRecordType
    {
        /// <summary>
        /// A mail destination (OBSOLETE - use MX).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035">RFC 1035</seealso>
        [Obsolete("Use MX")]
        public static readonly ResourceRecordType Instance = new ResourceRecordType(
            abbreviation: "Md",
            value: 3
        );
    }
}