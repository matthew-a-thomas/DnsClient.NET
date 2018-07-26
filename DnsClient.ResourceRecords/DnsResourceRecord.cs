namespace DnsClient.ResourceRecords
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Base class for all resource records.
    /// </summary>
    /// <seealso cref="ResourceRecord" />
    public abstract class DnsResourceRecord : ResourceRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DnsResourceRecord" /> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="info"/> is null.</exception>
        public DnsResourceRecord(ResourceRecord info)
            : base(
                  info?.DomainName ?? throw new ArgumentNullException(nameof(info)),
                  info.RecordType,
                  info.RecordClass,
                  info.TimeToLive,
                  info.RawDataLength)
        {
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(0);
        }

        /// <summary>
        /// Same as <c>ToString</c> but offsets the <see cref="ResourceRecord.DomainName"/>
        /// by <paramref name="offset"/>.
        /// Set the offset to -32 for example to make it print nicely in consols.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>A string representing this instance.</returns>
        [SuppressMessage("ReSharper",
            "FormatStringProblem")]
        public string ToString(int offset)
        {
            return string.Format("{0," + offset + "}{1} \t{2} \t{3} \t{4}",
                DomainName,
                TimeToLive,
                RecordClass,
                RecordType,
                RecordToString());
        }

        /// <summary>
        /// Clones this <see cref="DnsResourceRecord"/>.
        /// </summary>
        public DnsResourceRecord Clone()
        {
            return (DnsResourceRecord)MemberwiseClone();
        }

        /// <summary>
        /// Returns a string representation of the record's value only.
        /// <see cref="ToString(int)"/> uses this to compose the full string value of this instance.
        /// </summary>
        /// <returns>A string representing this record.</returns>
        protected abstract string RecordToString();
    }
}