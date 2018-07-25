namespace DnsClient.Tests
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Protocol;
    using Moq;
    using Protocol;
    using Xunit;

    public class MockExampleTest
    {
        [Fact]
        public async Task MockLookup()
        {
            // arrange
            var lookupMock = new Mock<IDnsQuery>();

            var aRecord = new ARecord(new ResourceRecordInfo("query", ResourceRecordType.A, QueryClass.In, 0, 0), IPAddress.Any);

            var responseMsg = new DnsResponseMessage(new DnsResponseHeader(123, 256, 1, 1, 0, 1), 123);
            responseMsg.Answers.Add(aRecord);
            IDnsQueryResponse dnsResponse = responseMsg.AsQueryResponse(new NameServer(NameServer.GooglePublicDns));

            //// or mock response
            //var dnsResponseMock = new Mock<IDnsQueryResponse>();
            //dnsResponseMock
            //    .Setup(p => p.Answers)
            //        .Returns(new DnsResourceRecord[] { aRecord });

            var response = Task.FromResult(dnsResponse);
            lookupMock.Setup(f => f.QueryAsync(It.IsAny<string>(), QueryType.A, QueryClass.In, new CancellationToken())).Returns(response);
            var lookup = lookupMock.Object;

            // act
            var result = await lookup.QueryAsync("query", QueryType.A);

            // assert
            Assert.Equal(1, result.Header.AnswerCount);
            Assert.Equal("query.", result.Answers.First().DomainName.Value);
            Assert.Equal(IPAddress.Any, result.Answers.ARecords().First().Address);
        }
    }
}