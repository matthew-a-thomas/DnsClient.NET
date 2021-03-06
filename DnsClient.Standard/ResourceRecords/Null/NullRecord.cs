﻿namespace DnsClient.Standard.ResourceRecords.Null
{
    using System;
    using System.Text;
    using Core;
    using DnsClient.ResourceRecords;

    /*
    https://tools.ietf.org/html/rfc1035#section-3.3.10:
    3.3.10. NULL RDATA format (EXPERIMENTAL)

        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        /                  <anything>                   /
        /                                               /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

    Anything at all may be in the RDATA field so long as it is 65535 octets
    or less.
     */

    /// <summary>
    /// Experimental RR, not sure if the implementation is actually correct either (not tested).
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.10">RFC 1035</seealso>
    public class NullRecord : DnsResourceRecord
    {
        /// <summary>
        /// A Null resource record (EXPERIMENTAL).
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.8">RFC 1035</seealso>
        /// <seealso cref="NullRecord"/>
        public static readonly PseudoResourceRecordType ResourceRecordType = new PseudoResourceRecordType(
            abbreviation: "Null",
            value: 10
        );

        /// <summary>
        /// Gets any data stored in this record.
        /// </summary>
        /// <value>
        /// The byte array.
        /// </value>
        public byte[] Anything { get; }

        /// <summary>
        /// Gets the raw data of this record as UTF8 string.
        /// </summary>
        public string AsString { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullRecord" /> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="anything">Anything.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="info"/> or <paramref name="anything"/> is null.</exception>
        public NullRecord(ResourceRecord info, byte[] anything)
            : base(info)
        {
            Anything = anything ?? throw new ArgumentNullException(nameof(anything));
            try
            {
                AsString = Encoding.UTF8.GetString(anything);
            }
            catch
            {
                // ignore errors.
            }
        }

        protected override string RecordToString()
        {
            return $"\\# {Anything.Length} {AsString}";
        }
    }
}