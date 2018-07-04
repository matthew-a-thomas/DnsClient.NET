namespace DnsClient
{
    using System.Collections.Generic;

    internal static class DnsResponseCodeText
    {
        internal const string Badalg = "Algorithm not supported";
        internal const string Badcookie = "Bad/missing Server Cookie";
        internal const string Badkey = "Key not recognized";
        internal const string Badmode = "Bad TKEY Mode";
        internal const string Badname = "Duplicate key name";
        internal const string Badsig = "TSIG Signature Failure";
        internal const string Badtime = "Signature out of time window";
        internal const string Badtrunc = "Bad Truncation";
        internal const string Badvers = "Bad OPT Version";
        internal const string FormErr = "Format Error";
        internal const string NoError = "No Error";
        internal const string NotAuth = "Server Not Authoritative for zone or Not Authorized";
        internal const string NotImp = "Not Implemented";
        internal const string NotZone = "Name not contained in zone";
        internal const string NxDomain = "Non-Existent Domain";
        internal const string NxrrSet = "RR Set that should exist does not";
        internal const string Refused = "Query Refused";
        internal const string ServFail = "Server Failure";
        internal const string Unassigned = "Unknown Error";
        internal const string YxDomain = "Name Exists when it should not";
        internal const string YxrrSet = "RR Set Exists when it should not";

        private static readonly Dictionary<DnsResponseCode, string> Errors = new Dictionary<DnsResponseCode, string>
        {
            { DnsResponseCode.NoError, NoError },
            { DnsResponseCode.FormatError, FormErr },
            { DnsResponseCode.ServerFailure, ServFail },
            { DnsResponseCode.NotExistentDomain, NxDomain },
            { DnsResponseCode.NotImplemented, NotImp },
            { DnsResponseCode.Refused, Refused },
            { DnsResponseCode.ExistingDomain, YxDomain },
            { DnsResponseCode.ExistingResourceRecordSet, YxrrSet },
            { DnsResponseCode.MissingResourceRecordSet, NxrrSet },
            { DnsResponseCode.NotAuthorized, NotAuth },
            { DnsResponseCode.NotZone, NotZone },
            { DnsResponseCode.BadVersionOrBadSignature, Badvers },
            { DnsResponseCode.BadKey, Badkey },
            { DnsResponseCode.BadTime, Badtime },
            { DnsResponseCode.BadMode, Badmode },
            { DnsResponseCode.BadName, Badname },
            { DnsResponseCode.BadAlgorithm, Badalg },
            { DnsResponseCode.BadTruncation, Badtrunc },
            { DnsResponseCode.BadCookie, Badcookie }
        };

        public static string GetErrorText(DnsResponseCode code)
        {
            if (!Errors.ContainsKey(code))
            {
                return Unassigned;
            }

            return Errors[code];
        }
    }
}