﻿namespace DnsClient
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Standard;
    using Standard.ResourceRecords.Opt;

    internal abstract class DnsMessageHandler
    {
        private readonly IReadOnlyDictionary<ushort, PseudoResourceRecordType> _resourceRecordTypes =
            new StandardResourceRecordTypesProvider().ResourceRecordTypes;

        private readonly DnsRecordFactory _dnsRecordFactory = new StandardDnsRecordFactoryFactory().Create();
        private readonly IReadOnlyDictionary<ushort, PseudoResourceRecordType> _queryTypes
            = new StandardQueryTypesProvider().QueryTypes;

        public abstract DnsResponseMessage Query(IPEndPoint endpoint, DnsRequestMessage request, TimeSpan timeout);

        public abstract Task<DnsResponseMessage> QueryAsync(IPEndPoint server, DnsRequestMessage request, CancellationToken cancellationToken,
            Action<Action> cancelationCallback);

        public abstract bool IsTransientException<T>(T exception) where T : Exception;

        protected static void GetRequestData(DnsRequestMessage request, DnsDatagramWriter writer)
        {
            var question = request.Question;

            /*
                                                1  1  1  1  1  1
                  0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
                +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
                |                      ID                       |
                +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
                |QR|   Opcode  |AA|TC|RD|RA|   Z    |   RCODE   |
                +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
                |                    QDCOUNT                    |
                +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
                |                    ANCOUNT                    |
                +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
                |                    NSCOUNT                    |
                +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
                |                    ARCOUNT                    |
                +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
            * */
            // 4 more bytes for the type and class

            writer.WriteInt16NetworkOrder((short)request.Header.Id);
            writer.WriteUInt16NetworkOrder(request.Header.RawFlags);
            writer.WriteInt16NetworkOrder(1);   // we support single question only... (as most DNS servers anyways).
            writer.WriteInt16NetworkOrder(0);
            writer.WriteInt16NetworkOrder(0);
            writer.WriteInt16NetworkOrder(1); // one additional for the Opt record.

            writer.WriteHostName(question.QueryName);
            writer.WriteUInt16NetworkOrder(question.QuestionType.Value);
            writer.WriteUInt16NetworkOrder((ushort)question.QuestionClass);

            /*
               +------------+--------------+------------------------------+
               | Field Name | Field Type   | Description                  |
               +------------+--------------+------------------------------+
               | NAME       | domain name  | MUST be 0 (root domain)      |
               | TYPE       | u_int16_t    | OPT (41)                     |
               | CLASS      | u_int16_t    | requestor's UDP payload size |
               | TTL        | u_int32_t    | extended RCODE and flags     |
               | RDLEN      | u_int16_t    | length of all RDATA          |
               | RDATA      | octet stream | {attribute,value} pairs      |
               +------------+--------------+------------------------------+
            * */

            var opt = new OptRecord();

            writer.WriteHostName("");
            writer.WriteUInt16NetworkOrder(opt.RecordType.Value);
            writer.WriteUInt16NetworkOrder((ushort)opt.RecordClass);
            writer.WriteUInt32NetworkOrder((ushort)opt.TimeToLive);
            writer.WriteUInt16NetworkOrder(0);
        }

        /// <summary>
        /// Reads a <see cref="ResourceRecord"/> from this <see cref="DnsDatagramReader"/>.
        /// </summary>
        private ResourceRecord ReadRecordInfo(DnsDatagramReader reader)
        {
            var queryString = reader.ReadQuestionQueryString();
            var type = reader.ReadUInt16NetworkOrder();
            if (!_resourceRecordTypes.TryGetValue(type, out var resourceRecordType))
                resourceRecordType = new PseudoResourceRecordType(
                    abbreviation: "Unknown - machine generated",
                    value: type
                );
            var queryClass = reader.ReadUInt16NetworkOrder();
            var ttl = reader.ReadUInt32NetworkOrder();
            var rdLength = reader.ReadUInt16NetworkOrder();
            return new ResourceRecord(
                queryString,                      // name
                resourceRecordType,   // type
                (QueryClass)queryClass,           // class
                (int)ttl,                  // ttl - 32bit!!
                rdLength);                      // RDLength
        }

        public DnsResponseMessage GetResponseMessage(ArraySegment<byte> responseData)
        {
            var reader = new DnsDatagramReader(responseData);

            var id = reader.ReadUInt16NetworkOrder();
            var flags = reader.ReadUInt16NetworkOrder();
            var questionCount = reader.ReadUInt16NetworkOrder();
            var answerCount = reader.ReadUInt16NetworkOrder();
            var nameServerCount = reader.ReadUInt16NetworkOrder();
            var additionalCount = reader.ReadUInt16NetworkOrder();

            var header = new DnsResponseHeader(id, flags, questionCount, answerCount, additionalCount, nameServerCount);
            var response = new DnsResponseMessage(header, responseData.Count);

            for (var questionIndex = 0; questionIndex < questionCount; questionIndex++)
            {
                var questionQueryString = reader.ReadQuestionQueryString();
                var queryTypeValue = reader.ReadUInt16NetworkOrder();
                var queryClass = reader.ReadUInt16NetworkOrder();
                if (!_queryTypes.TryGetValue(queryTypeValue, out var queryType))
                    queryType = new PseudoResourceRecordType(
                        abbreviation: "Unknown",
                        value: queryTypeValue
                    );
                var question = new DnsQuestion(questionQueryString, queryType, (QueryClass)queryClass);
                response.AddQuestion(question);
            }

            for (var answerIndex = 0; answerIndex < answerCount; answerIndex++)
            {
                var info = ReadRecordInfo(reader);
                var record = _dnsRecordFactory.GetRecord(info, reader);
                response.AddAnswer(record);
            }

            for (var serverIndex = 0; serverIndex < nameServerCount; serverIndex++)
            {
                var info = ReadRecordInfo(reader);
                var record = _dnsRecordFactory.GetRecord(info, reader);
                response.AddAuthority(record);
            }

            for (var additionalIndex = 0; additionalIndex < additionalCount; additionalIndex++)
            {
                var info = ReadRecordInfo(reader);
                var record = _dnsRecordFactory.GetRecord(info, reader);
                response.AddAdditional(record);
            }

            return response;
        }
    }
}