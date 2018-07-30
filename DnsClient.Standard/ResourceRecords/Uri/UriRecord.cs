﻿namespace DnsClient.Standard.ResourceRecords.Uri
{
    using System;
    using Core;
    using DnsClient.ResourceRecords;

    /*
    * RFC 7553  https://tools.ietf.org/html/rfc7553

    4.5.  URI RDATA Wire Format

        The RDATA for a URI RR consists of a 2-octet Priority field, a
        2-octet Weight field, and a variable-length Target field.

        Priority and Weight are unsigned integers in network byte order.

        The remaining data in the RDATA contains the Target field.  The
        Target field contains the URI as a sequence of octets (without the
        enclosing double-quote characters used in the presentation format).

        The length of the Target field MUST be greater than zero.

                            1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
        0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
        +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        |          Priority             |          Weight               |
        +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        /                                                               /
        /                             Target                            /
        /                                                               /
        +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    */

    /// <summary>
    /// A <see cref="DnsResourceRecord"/> represending a Uniform Resource Identifier (URI) resource.
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc7553">RFC 7553</seealso>
    public class UriRecord : DnsResourceRecord
    {
        /// <summary>
        /// A Uniform Resource Identifier (URI) resource record.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc7553">RFC 7553</seealso>
        /// <seealso cref="UriRecord"/>
        public static readonly PseudoResourceRecordType ResourceRecordType = new PseudoResourceRecordType(
            abbreviation: "Uri",
            value: 256
        );

        /// <summary>
        /// Gets or sets the target Uri.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the weigth.
        /// </summary>
        /// <value>
        /// The weigth.
        /// </value>
        public int Weigth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriRecord"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="info"/> or <paramref name="target"/> is null.</exception>
        [CLSCompliant(false)]
        public UriRecord(ResourceRecord info, ushort priority, ushort weight, string target)
            : base(info)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Priority = priority;
            Weigth = weight;
        }

        protected override string RecordToString()
        {
            return $"{Priority} {Weigth} \"{Target}\"";
        }
    }
}