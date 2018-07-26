namespace DnsClient.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Core;
    using ResourceRecords;
    using ResourceRecords.A;
    using ResourceRecords.Aaaa;
    using ResourceRecords.Mb;
    using ResourceRecords.Mx;
    using ResourceRecords.Ns;
    using ResourceRecords.Ptr;
    using ResourceRecords.Soa;
    using ResourceRecords.Srv;
    using ResourceRecords.Sshfp;
    using ResourceRecords.Txt;
    using Xunit;

    [SuppressMessage("ReSharper",
        "ConvertToLocalFunction")]
    public class DnsRecordFactoryTest
    {
        private static DnsRecordFactory GetFactory(byte[] data, out DnsDatagramReader reader)
        {
            reader = new DnsDatagramReader(new ArraySegment<byte>(data));
            return new StandardDnsRecordFactoryFactory().Create();
        }

        [Fact]
        public void DnsRecordFactory_PTRRecordNotEnoughData()
        {
            var data = new byte[0];
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Ptr, QueryClass.In, 0, data.Length);

            Action act = () => factory.GetRecord(info, reader);

            Assert.ThrowsAny<IndexOutOfRangeException>(act);
        }

        [Fact]
        public void DnsRecordFactory_PTRRecordEmptyName()
        {
            var data = new byte[] { 0 };
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Ptr, QueryClass.In, 0, data.Length);

            var result = factory.GetRecord(info, reader) as PtrRecord ?? throw new Exception();

            Assert.Equal(".", result.PtrDomainName.Value);
        }

        [Fact]
        public void DnsRecordFactory_PTRRecord()
        {
            var name = DnsString.Parse("result.example.com");
            var writer = new DnsDatagramWriter();
            writer.WriteHostName(name.Value);
            var factory = GetFactory(writer.Data.ToArray(), out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Ptr, QueryClass.In, 0, writer.Data.Count);

            var result = factory.GetRecord(info, reader) as PtrRecord ?? throw new Exception();

            Assert.Equal(result.PtrDomainName, name);
        }

        [Fact]
        public void DnsRecordFactory_MBRecord()
        {
            var name = DnsString.Parse("Müsli.de");
            var writer = new DnsDatagramWriter();
            writer.WriteHostName(name.Value);
            var factory = GetFactory(writer.Data.ToArray(), out var reader);
            var info = new ResourceRecordInfo("Müsli.de", ResourceRecordType.Mb, QueryClass.In, 0, writer.Data.Count);

            var result = factory.GetRecord(info, reader) as MbRecord ?? throw new Exception();

            Assert.Equal(result.MadName, name);
            Assert.Equal("müsli.de.", result.MadName.Original);
        }

        [Fact]
        public void DnsRecordFactory_ARecordNotEnoughData()
        {
            var data = new byte[] { 23, 23, 23 };
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("example.com", ResourceRecordType.A, QueryClass.In, 0, data.Length);

            Action act = () => factory.GetRecord(info, reader);

            Assert.ThrowsAny<IndexOutOfRangeException>(act);
        }

        [Fact]
        public void DnsRecordFactory_ARecord()
        {
            var data = new byte[] { 23, 24, 25, 26 };
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("example.com", ResourceRecordType.A, QueryClass.In, 0, data.Length);

            var result = factory.GetRecord(info, reader) as ARecord ?? throw new Exception();

            Assert.Equal(result.Address, IPAddress.Parse("23.24.25.26"));
        }

        [Fact]
        public void DnsRecordFactory_AAAARecordNotEnoughData()
        {
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("example.com", ResourceRecordType.Aaaa, QueryClass.In, 0, data.Length);

            Action act = () => factory.GetRecord(info, reader);

            Assert.ThrowsAny<IndexOutOfRangeException>(act);
        }

        [Fact]
        public void DnsRecordFactory_AAAARecord()
        {
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("example.com", ResourceRecordType.Aaaa, QueryClass.In, 0, data.Length);

            var result = factory.GetRecord(info, reader) as AaaaRecord ?? throw new Exception();

            Assert.Equal(result.Address, IPAddress.Parse("102:304:506:708:90a:b0c:d0e:f10"));
            Assert.Equal(result.Address.GetAddressBytes(), data);
        }

        [Fact]
        public void DnsRecordFactory_NSRecordNotEnoughData()
        {
            var data = new byte[0];
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Ns, QueryClass.In, 0, data.Length);

            Action act = () => factory.GetRecord(info, reader);

            Assert.ThrowsAny<IndexOutOfRangeException>(act);
        }

        [Fact]
        public void DnsRecordFactory_NSRecordEmptyName()
        {
            var data = new byte[] { 0 };
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Ns, QueryClass.In, 0, data.Length);

            var result = factory.GetRecord(info, reader) as NsRecord ?? throw new Exception();

            Assert.Equal(".", result.NsdName.Value);
        }

        [Fact]
        public void DnsRecordFactory_NSRecord()
        {
            var writer = new DnsDatagramWriter();
            var name = DnsString.Parse("result.example.com");
            writer.WriteHostName(name.Value);
            var factory = GetFactory(writer.Data.ToArray(), out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Ns, QueryClass.In, 0, writer.Data.Count);

            var result = factory.GetRecord(info, reader) as NsRecord ?? throw new Exception();

            Assert.Equal(result.NsdName, name);
        }

        [Fact]
        public void DnsRecordFactory_MXRecordOrderMissing()
        {
            var data = new byte[0];
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Mx, QueryClass.In, 0, data.Length);

            Action act = () => factory.GetRecord(info, reader);

            Assert.ThrowsAny<IndexOutOfRangeException>(act);
        }

        [Fact]
        public void DnsRecordFactory_MXRecordNameMissing()
        {
            var data = new byte[] { 1, 2 };
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Mx, QueryClass.In, 0, data.Length);

            Action act = () => factory.GetRecord(info, reader);

            Assert.ThrowsAny<IndexOutOfRangeException>(act);
        }

        [Fact]
        public void DnsRecordFactory_MXRecordEmptyName()
        {
            var data = new byte[] { 1, 0, 0 };
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Mx, QueryClass.In, 0, data.Length);

            var result = factory.GetRecord(info, reader) as MxRecord ?? throw new Exception();

            Assert.Equal(256, result.Preference);
            Assert.Equal(".", result.Exchange.Value);
        }

        [Fact]
        public void DnsRecordFactory_MXRecord()
        {
            var name = DnsString.Parse("result.example.com");
            var writer = new DnsDatagramWriter();
            writer.WriteByte(0);
            writer.WriteByte(1);
            writer.WriteHostName(name.Value);

            var factory = GetFactory(writer.Data.ToArray(), out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Mx, QueryClass.In, 0, writer.Data.Count);

            var result = factory.GetRecord(info, reader) as MxRecord ?? throw new Exception();

            Assert.Equal(1, result.Preference);
            Assert.Equal(result.Exchange, name);
        }

        [Fact]
        public void DnsRecordFactory_SOARecordEmpty()
        {
            var data = new byte[0];
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Soa, QueryClass.In, 0, data.Length);

            Action act = () => factory.GetRecord(info, reader);

            Assert.ThrowsAny<IndexOutOfRangeException>(act);
        }

        [Fact]
        public void DnsRecordFactory_SOARecord()
        {
            var data = new byte[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 4, 0, 0, 0, 5 };
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Soa, QueryClass.In, 0, data.Length);

            var result = factory.GetRecord(info, reader) as SoaRecord ?? throw new Exception();

            Assert.Equal(".", result.MName.Value);
            Assert.Equal(".", result.RName.Value);
            Assert.True(result.Serial == 1);
            Assert.True(result.Refresh == 2);
            Assert.True(result.Retry == 3);
            Assert.True(result.Expire == 4);
            Assert.True(result.Minimum == 5);
        }

        [Fact]
        public void DnsRecordFactory_SRVRecordEmpty()
        {
            var data = new byte[0];
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Srv, QueryClass.In, 0, data.Length);

            Action act = () => factory.GetRecord(info, reader);

            Assert.ThrowsAny<IndexOutOfRangeException>(act);
        }

        [Fact]
        public void DnsRecordFactory_SRVRecord()
        {
            var name = DnsString.Parse("result.example.com");
            var writer = new DnsDatagramWriter();
            writer.WriteBytes(new byte[] { 0, 1, 1, 0, 2, 3 }, 6);
            writer.WriteHostName(name.Value);
            var factory = GetFactory(writer.Data.ToArray(), out var reader);

            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Srv, QueryClass.In, 0, writer.Data.Count);

            var result = factory.GetRecord(info, reader) as SrvRecord ?? throw new Exception();

            Assert.Equal(result.Target, name);
            Assert.True(result.Priority == 1);
            Assert.True(result.Weight == 256);
            Assert.True(result.Port == 515);
        }

        [Fact]
        public void DnsRecordFactory_TXTRecordEmpty()
        {
            const string textA = "Some Text";
            var lineA = Encoding.ASCII.GetBytes(textA);
            var data = new List<byte> {5};
            data.AddRange(lineA);

            var factory = GetFactory(data.ToArray(), out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Txt, QueryClass.In, 0, data.Count);

            Action act = () => factory.GetRecord(info, reader);

            Assert.ThrowsAny<IndexOutOfRangeException>(act);
        }

        [Fact]
        public void DnsRecordFactory_TXTRecordWrongTextLength()
        {
            var data = new byte[0];
            var factory = GetFactory(data, out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Txt, QueryClass.In, 0, data.Length);

            var result = factory.GetRecord(info, reader) as TxtRecord ?? throw new Exception();

            Assert.Empty(result.EscapedText);
        }

        [Fact]
        public void DnsRecordFactory_TXTRecord()
        {
            const string textA = @"Some lines of text.";
            const string textB = "Another line";
            var lineA = Encoding.ASCII.GetBytes(textA);
            var lineB = Encoding.ASCII.GetBytes(textB);
            var data = new List<byte> {(byte) lineA.Length};
            data.AddRange(lineA);
            data.Add((byte)lineB.Length);
            data.AddRange(lineB);

            var factory = GetFactory(data.ToArray(), out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Txt, QueryClass.In, 0, data.Count);

            var result = factory.GetRecord(info, reader) as TxtRecord ?? throw new Exception();

            Assert.Equal(2, result.EscapedText.Count);
            Assert.Equal(result.EscapedText.ElementAt(0), textA);
            Assert.Equal(result.EscapedText.ElementAt(1), textB);
        }

        [Fact]
        public void DnsRecordFactory_SSHFPRecord()
        {
            var algo = SshfpAlgorithm.Rsa;
            var type = SshfpFingerprintType.Sha1;
            var fingerprint = "9DBA55CEA3B8E15528665A6781CA7C35190CF0EC";
            // Value is stored as raw bytes in the record, so convert the HEX string above to it's original bytes
            var fingerprintBytes = Enumerable.Range(0, fingerprint.Length / 2)
                .Select(i => Convert.ToByte(fingerprint.Substring(i * 2, 2), 16));

            var data = new List<byte>();
            data.Add((byte)algo);
            data.Add((byte)type);
            data.AddRange(fingerprintBytes);

            var factory = GetFactory(data.ToArray(), out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Sshfp, QueryClass.In, 0, data.Count);

            var result = factory.GetRecord(info, reader) as SshfpRecord;

            Assert.NotNull(result);
            Assert.Equal(SshfpAlgorithm.Rsa, result.Algorithm);
            Assert.Equal(SshfpFingerprintType.Sha1, result.FingerprintType);
            Assert.Equal(fingerprint, result.Fingerprint);
        }

        [Fact]
        public void DnsRecordFactory_SpecialChars()
        {
            const string textA = "\"äöü \\slash/! @bla.com \"";
            const string textB = "(Another line)";
            var lineA = Encoding.UTF8.GetBytes(textA);
            var lineB = Encoding.UTF8.GetBytes(textB);
            var data = new List<byte> {(byte) lineA.Length};
            data.AddRange(lineA);
            data.Add((byte)lineB.Length);
            data.AddRange(lineB);

            var factory = GetFactory(data.ToArray(), out var reader);
            var info = new ResourceRecordInfo("query.example.com", ResourceRecordType.Txt, QueryClass.In, 0, data.Count);

            var result = factory.GetRecord(info, reader) as TxtRecord ?? throw new Exception();

            Assert.Equal(2, result.EscapedText.Count);
            Assert.Equal(result.Text.ElementAt(0), textA);
            Assert.Equal("\\\"\\195\\164\\195\\182\\195\\188 \\\\slash/! @bla.com \\\"", result.EscapedText.ElementAt(0));
            Assert.Equal(result.Text.ElementAt(1), textB);
            Assert.Equal(result.EscapedText.ElementAt(1), textB);
        }
    }
}