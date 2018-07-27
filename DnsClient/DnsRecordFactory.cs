namespace DnsClient
{
    using System;
    using System.Collections.Generic;
    using Core;
    using ResourceRecords;

    public sealed class DnsRecordFactory
    {
        private readonly IReadOnlyDictionary<PseudoResourceRecordType, IResourceRecordReader<DnsResourceRecord>> _recordReaders;

        public DnsRecordFactory(
            IReadOnlyDictionary<PseudoResourceRecordType, IResourceRecordReader<DnsResourceRecord>> recordReaders
        )
        {
            _recordReaders = recordReaders;
        }

        /*
        0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                                               |
        /                                               /
        /                      NAME                     /
        |                                               |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                      TYPE                     |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                     CLASS                     |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                      TTL                      |
        |                                               |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                   RDLENGTH                    |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--|
        /                     RDATA                     /
        /                                               /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
         * */

        public DnsResourceRecord GetRecord(
            ResourceRecord info,
            DnsDatagramReader reader)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            var oldIndex = reader.Index;
            DnsResourceRecord result;

            if (_recordReaders.TryGetValue(info.RecordType, out var recordReader))
            {
                result = recordReader.ReadResourceRecord(info, reader);
            }
            else
            {
                // update reader index because we don't read full data for the empty record
                reader.Index += info.RawDataLength;
                result = new EmptyRecord(info);
            }

            // sanity check
            if (reader.Index != oldIndex + info.RawDataLength)
            {
                throw new InvalidOperationException("Record reader index out of sync.");
            }

            return result;
        }
    }
}