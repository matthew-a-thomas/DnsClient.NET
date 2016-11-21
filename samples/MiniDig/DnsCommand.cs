﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DnsClient;
using DnsClient.Protocol.Record;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace DigApp
{
    internal abstract class DnsCommand
    {
        public CommandOption ConnectTimeoutArg { get; private set; }

        public CommandOption LogLevelArg { get; private set; }

        public CommandOption NoRecurseArg { get; private set; }

        public string[] OriginalArgs { get; }

        public CommandOption PortArg { get; private set; }

        public CommandOption ServerArg { get; private set; }

        public CommandOption TriesArg { get; private set; }

        public CommandOption UseTcpArg { get; private set; }

        protected CommandLineApplication App { get; }

        public DnsCommand(CommandLineApplication app, string[] originalArgs)
        {
            App = app;
            OriginalArgs = originalArgs;
            App.OnExecute(() => Execute());
            Configure();
        }

        public LookupClient GetDnsLookup()
        {
            if (UseTcp())
            {
                throw new NotImplementedException();
            }
            else
            {
                return new LookupClient(GetEndpointsValue())
                {
                    Recursion = GetUseRecursionValue(),
                    Retries = GetTriesValue(),
                    Timeout = TimeSpan.FromMilliseconds(GetTimeoutValue())
                };
            }
        }

        public DnsEndPoint[] GetEndpointsValue()
        {
            if (ServerArg.HasValue())
            {
                string server = ServerArg.Value();
                IPAddress ip;
                if (!IPAddress.TryParse(server, out ip))
                {
                    try
                    {
                        var lookup = new LookupClient();
                        var result = lookup.QueryAsync(server, QueryType.A).Result;
                        ip = result.Answers.OfType<ARecord>().FirstOrDefault()?.Address;
                    }
                    catch
                    {
                        throw new Exception("Cannot resolve server.");
                    }
                }

                return new[] { new DnsEndPoint(ip.ToString(), GetPortValue()) };
            }
            else
            {
                return NameServer.ResolveNameServers().ToArray();
            }
        }

        public LogLevel GetLoglevelValue()
        {
            LogLevel logginglevel;
            return LogLevelArg.HasValue() && Enum.TryParse(LogLevelArg.Value(), true, out logginglevel) ? logginglevel : LogLevel.Warning;
        }

        public int GetPortValue() => PortArg.HasValue() ? int.Parse(PortArg.Value()) : 53;

        public int GetTimeoutValue() => ConnectTimeoutArg.HasValue() ? int.Parse(ConnectTimeoutArg.Value()) : 1000;

        public bool UseTcp() => UseTcpArg.HasValue() ? true : false;

        public int GetTriesValue() => TriesArg.HasValue() ? int.Parse(TriesArg.Value()) : 3;

        public bool GetUseRecursionValue() => !NoRecurseArg.HasValue();

        public bool GetUseTcpValue() => UseTcpArg.HasValue();

        protected virtual void Configure()
        {
            ServerArg = App.Option(
                "-s | --server",
                "The DNS server [local network].",
                CommandOptionType.SingleValue);

            PortArg = App.Option(
                "-p | --port",
                $"The port to use to connect to the DNS server [{NameServer.DefaultPort}].",
                CommandOptionType.SingleValue);

            NoRecurseArg = App.Option(
                "-nr | --norecurse",
                "Non recurive mode.",
                CommandOptionType.NoValue);

            UseTcpArg = App.Option(
                "--tcp",
                "Use TCP connection [Udp].",
                CommandOptionType.NoValue);

            TriesArg = App.Option(
                "--tries",
                "Number of tries [3].",
                CommandOptionType.SingleValue);

            ConnectTimeoutArg = App.Option(
                "--time",
                "Query timeout [1000].",
                CommandOptionType.SingleValue);

            LogLevelArg = App.Option(
                "--log-level",
                "Sets the log level [Warning].",
                CommandOptionType.SingleValue);

            App.HelpOption("-? | -h | --help");
        }

        protected abstract Task<int> Execute();
    }
}