﻿namespace DnsClient.Standard.ResourceRecords.Sshfp
{
    using System.Linq;
    using Core;

    public sealed class SshfpReader : IResourceRecordReader<SshfpRecord>
    {
        public PseudoResourceRecordType ResourceRecordType { get; } = SshfpRecord.ResourceRecordType;

        public SshfpRecord ReadResourceRecord(
            ResourceRecord info,
            DnsDatagramReader reader)
        {
            var algorithm = (SshfpAlgorithm)reader.ReadByte();
            var fingerprintType = (SshfpFingerprintType)reader.ReadByte();
            var fingerprint = reader.ReadBytes(info.RawDataLength - 2).ToArray();
            var fingerprintHexString = string.Join(string.Empty, fingerprint.Select(b => b.ToString("X2")));
            return new SshfpRecord(info, algorithm, fingerprintType, fingerprintHexString);
        }
    }
}