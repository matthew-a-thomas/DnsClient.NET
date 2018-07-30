namespace DnsClient.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A pseudo resource record type used to identify any <see cref="ResourceRecord"/> or pseudo resource record.
    /// </summary>
    /// <remarks>
    /// <see cref="PseudoResourceRecordType"/>s are equal to one another if their <see cref="Value"/>s are equal.
    ///
    /// RFC 1035 (https://tools.ietf.org/html/rfc1035#section-3.2.2) defines a few resource record types.
    /// </remarks>
    public sealed class PseudoResourceRecordType : IEquatable<PseudoResourceRecordType>
    {
        public PseudoResourceRecordType(
            string abbreviation,
            ushort value)
        {
            Value = value;
            Abbreviation = abbreviation;
        }

        /// <summary>
        /// A human-readable abbreviation for this <see cref="PseudoResourceRecordType"/>.
        /// </summary>
        [SuppressMessage("ReSharper",
            "MemberCanBePrivate.Global")]
        public string Abbreviation { get; }

        /// <summary>
        /// The value that is used in DNS queries and responses.
        /// </summary>
        public ushort Value { get; }

        public override string ToString() => Abbreviation;

        public bool Equals(
            PseudoResourceRecordType other)
        {
            if (ReferenceEquals(
                null,
                other))
                return false;
            if (ReferenceEquals(
                this,
                other))
                return true;
            return Value == other.Value;
        }

        public override bool Equals(
            object obj) => obj is PseudoResourceRecordType type && Equals(type);

        public override int GetHashCode() => Value.GetHashCode();
    }
}