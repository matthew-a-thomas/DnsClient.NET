namespace DnsClient.Standard.ResourceRecords.HInfo
{
    using Core;
    using DnsClient.ResourceRecords;

    /*
    https://tools.ietf.org/html/rfc1035#section-3.3.11:
    3.3.2. HINFO RDATA format

        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        /                      CPU                      /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        /                       OS                      /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

    where:

    CPU             A <character-string> which specifies the CPU type.

    OS              A <character-string> which specifies the operating
                    system type.
     */

    /// <summary>
    /// A <see cref="DnsResourceRecord"/> used to acquire general information about a host.
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
    /// <seealso href="https://tools.ietf.org/html/rfc1010">RFC 1010</seealso>
    public class HInfoRecord : DnsResourceRecord
    {
        /// <summary>
        /// Host information.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc1035#section-3.3.11">RFC 1035</seealso>
        /// <seealso href="https://tools.ietf.org/html/rfc1010">RFC 1010</seealso>
        /// <seealso cref="HInfoRecord"/>
        public static readonly PseudoResourceRecordType ResourceRecordType = new PseudoResourceRecordType(
            abbreviation: "HInfo",
            value: 13
        );

        /// <summary>
        /// Gets a <c>string</c> which specifies the CPU type.
        /// </summary>
        /// <value>
        /// The cpu.
        /// </value>
        public string Cpu { get; }

        /// <summary>
        /// Gets a <c>string</c> which specifies the operating system type.
        /// </summary>
        /// <value>
        /// The os.
        /// </value>
        public string Os { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HInfoRecord"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="cpu">The cpu.</param>
        /// <param name="os">The os.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="info"/> is null.</exception>
        public HInfoRecord(ResourceRecord info, string cpu, string os)
            : base(info)
        {
            Cpu = cpu;
            Os = os;
        }

        protected override string RecordToString()
        {
            return $"\"{Cpu}\" \"{Os}\"";
        }
    }
}