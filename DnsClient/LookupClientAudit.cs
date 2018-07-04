namespace DnsClient
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;

    internal class LookupClientAudit
    {
        private const string CPlaceHolder = "$$REPLACEME$$";
        private static readonly int SPrintOffset = -32;
        private readonly StringBuilder _auditWriter = new StringBuilder();
        private Stopwatch _swatch;

        public void StartTimer()
        {
            _swatch = Stopwatch.StartNew();
            _swatch.Restart();
        }

        public void AuditResolveServers(int count)
        {
            _auditWriter.AppendLine($"; ({count} server found)");
        }

        public string Build(IDnsQueryResponse queryResponse)
        {
            var writer = new StringBuilder();

            if (queryResponse != null)
            {
                if (queryResponse.Questions.Count > 0)
                {
                    writer.AppendLine(";; QUESTION SECTION:");
                    foreach (var question in queryResponse.Questions)
                    {
                        writer.AppendLine(question.ToString(SPrintOffset));
                    }
                    writer.AppendLine();
                }

                if (queryResponse.Answers.Count > 0)
                {
                    writer.AppendLine(";; ANSWER SECTION:");
                    foreach (var answer in queryResponse.Answers)
                    {
                        writer.AppendLine(answer.ToString(SPrintOffset));
                    }
                    writer.AppendLine();
                }

                if (queryResponse.Authorities.Count > 0)
                {
                    writer.AppendLine(";; AUTHORITIES SECTION:");
                    foreach (var auth in queryResponse.Authorities)
                    {
                        writer.AppendLine(auth.ToString(SPrintOffset));
                    }
                    writer.AppendLine();
                }

                if (queryResponse.Additionals.Count > 0)
                {
                    writer.AppendLine(";; ADDITIONALS SECTION:");
                    foreach (var additional in queryResponse.Additionals)
                    {
                        writer.AppendLine(additional.ToString(SPrintOffset));
                    }
                    writer.AppendLine();
                }
            }

            var all = _auditWriter.ToString();
            var dynamic = writer.ToString();

            return all.Replace(CPlaceHolder, dynamic);
        }

        public void AuditTruncatedRetryTcp()
        {
            _auditWriter.AppendLine(";; Truncated, retrying in TCP mode.");
            _auditWriter.AppendLine();
        }

        public void AuditResponseError(DnsResponseCode responseCode)
        {
            _auditWriter.AppendLine($";; ERROR: {DnsResponseCodeText.GetErrorText(responseCode)}");
        }

        public void AuditOptPseudo()
        {
            _auditWriter.AppendLine(";; OPT PSEUDOSECTION:");
        }

        public void AuditResponseHeader(DnsResponseHeader header)
        {
            _auditWriter.AppendLine(";; Got answer:");
            _auditWriter.AppendLine(header.ToString());
            if (header.RecursionDesired && !header.RecursionAvailable)
            {
                _auditWriter.AppendLine(";; WARNING: recursion requested but not available");
            }
            _auditWriter.AppendLine();
        }

        public void AuditEdnsOpt(short udpSize, byte version, DnsResponseCode responseCodeEx)
        {
            // TODO: flags
            _auditWriter.AppendLine($"; EDNS: version: {version}, flags:; udp: {udpSize}");
        }

        public void AuditResponse()
        {
            _auditWriter.AppendLine(CPlaceHolder);
        }

        public void AuditEnd(DnsQueryResponse queryResponse)
        {
            var elapsed = _swatch.ElapsedMilliseconds;
            _auditWriter.AppendLine($";; Query time: {elapsed} msec");
            _auditWriter.AppendLine($";; SERVER: {queryResponse.NameServer.Endpoint.Address}#{queryResponse.NameServer.Endpoint.Port}");
            _auditWriter.AppendLine($";; WHEN: {DateTime.UtcNow.ToString("ddd MMM dd HH:mm:ss K yyyy", CultureInfo.InvariantCulture)}");
            _auditWriter.AppendLine($";; MSG SIZE  rcvd: {queryResponse.MessageSize}");
        }

        public void AuditException(Exception ex)
        {
            var aggEx = ex as AggregateException;
            if (ex is DnsResponseException dnsEx)
            {
                _auditWriter.AppendLine($";; Error: {DnsResponseCodeText.GetErrorText(dnsEx.Code)} {dnsEx.InnerException?.Message ?? dnsEx.Message}");
            }
            else if (aggEx != null)
            {
                _auditWriter.AppendLine($";; Error: {aggEx.InnerException?.Message ?? aggEx.Message}");
            }
            else
            {
                _auditWriter.AppendLine($";; Error: {ex.Message}");
            }

            if (Debugger.IsAttached)
            {
                _auditWriter.AppendLine(ex.ToString());
            }
        }

        public void AuditRetryNextServer(NameServer current)
        {
            _auditWriter.AppendLine();
            _auditWriter.AppendLine($"; SERVER: {current.Endpoint.Address}#{current.Endpoint.Port} failed; Retrying with the next server.");
        }
    }
}