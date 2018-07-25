﻿namespace DnsClient.Protocol
{
    using Core.Protocol;

    /// <summary>
    /// A <see cref="DnsResourceRecord"/> not representing any specifc resource record.
    /// Used if unsupported <see cref="ResourceRecordType"/>s are found in the result.
    /// </summary>
    /// <seealso cref="DnsResourceRecord" />
    public class EmptyRecord : DnsResourceRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyRecord"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="info"/> is null.</exception>
        public EmptyRecord(ResourceRecordInfo info) : base(info)
        {
        }

        protected override string RecordToString()
        {
            return string.Empty;
        }
    }
}