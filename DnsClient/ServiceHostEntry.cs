namespace DnsClient
{
    using System.Net;

    /// <summary>
    /// Extends <see cref="IPHostEntry"/> by the <see cref="ServiceHostEntry.Port"/> property.
    /// </summary>
    /// <seealso cref="System.Net.IPHostEntry" />
    public class ServiceHostEntry : IPHostEntry
    {
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; }
    }
}