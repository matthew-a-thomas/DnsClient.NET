namespace DnsClient.Standard.ResourceRecords.Aaaa
{
    using System.Net;
    using Core;
    using DnsClient.ResourceRecords;

    /// <summary>
    /// A <see cref="DnsResourceRecord"/> represending an IPv6 <see cref="IPAddress"/>.
    /// <para>
    /// A 128 bit IPv6 address is encoded in the data portion of an AAAA
    /// resource record in network byte order(high-order byte first).
    /// </para>
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc3596#section-2.2">RFC 3596</seealso>
    public class AaaaRecord : AddressRecord
    {
        /// <summary>
        /// An IPv6 host address.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc3596#section-2.2">RFC 3596</seealso>
        /// <seealso cref="AaaaRecord"/>
        public static readonly PseudoResourceRecordType ResourceRecordType = new PseudoResourceRecordType(
            abbreviation: "Aaaa",
            value: 28
        );

        /// <summary>
        /// Initializes a new instance of the <see cref="AaaaRecord"/> class.
        /// </summary>
        /// <inheritdoc />
        public AaaaRecord(ResourceRecord info, IPAddress address) : base(info, address)
        {
        }
    }
}