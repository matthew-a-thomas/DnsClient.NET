namespace DnsClient.ResourceRecords
{
    using Core;

    /// <summary>
    /// A <see cref="DnsResourceRecord"/> not representing any specifc resource record.
    /// Used if unsupported <see cref="PseudoResourceRecordType"/>s are found in the result.
    /// </summary>
    /// <seealso cref="DnsResourceRecord" />
    public class EmptyRecord : DnsResourceRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyRecord"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="info"/> is null.</exception>
        public EmptyRecord(ResourceRecord info) : base(info)
        {
        }

        protected override string RecordToString()
        {
            return string.Empty;
        }
    }
}