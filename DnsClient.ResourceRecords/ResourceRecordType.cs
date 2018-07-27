namespace DnsClient.ResourceRecords
{
    using System;

    /*
     * RFC 1035 (https://tools.ietf.org/html/rfc1035#section-3.2.2)
     * */

    /// <summary>
    /// A resource record type used to identify any <see cref="DnsResourceRecord"/>.
    /// <para>
    /// Resource record types are a subset of <see cref="QueryType"/>.
    /// </para>
    /// </summary>
    /// <seealso cref="DnsResourceRecord"/>
    /// <remarks>
    /// <see cref="ResourceRecordType"/>s are equal to one another if their <see cref="Value"/>s are equal.
    /// </remarks>
    public sealed class ResourceRecordType : IEquatable<ResourceRecordType>
    {
        public ResourceRecordType(
            string abbreviation,
            ushort value)
        {
            Value = value;
            Abbreviation = abbreviation;
        }

        /// <summary>
        /// A human-readable abbreviation for this <see cref="ResourceRecordType"/>.
        /// </summary>
        public string Abbreviation { get; }

        /// <summary>
        /// The value that is used in DNS queries and responses.
        /// </summary>
        public ushort Value { get; }

        public override string ToString() => Abbreviation;

        public bool Equals(
            ResourceRecordType other)
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
            object obj) => obj is ResourceRecordType type && Equals(type);

        public override int GetHashCode() => Value.GetHashCode();
    }
}