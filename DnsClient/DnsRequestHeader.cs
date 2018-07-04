namespace DnsClient
{
    internal class DnsRequestHeader
    {
        public const int HeaderLength = 12;

        private ushort _flags;

        public ushort RawFlags => _flags;

        internal DnsHeaderFlag HeaderFlags
        {
            get => (DnsHeaderFlag)_flags;
            set
            {
                _flags &= (ushort)~DnsHeaderFlag.IsCheckingDisabled;
                _flags &= (ushort)~DnsHeaderFlag.IsAuthenticData;
                _flags &= (ushort)~DnsHeaderFlag.FutureUse;
                _flags &= (ushort)~DnsHeaderFlag.HasQuery;
                _flags &= (ushort)~DnsHeaderFlag.HasAuthorityAnswer;
                _flags &= (ushort)~DnsHeaderFlag.ResultTruncated;
                _flags &= (ushort)~DnsHeaderFlag.RecursionDesired;
                _flags &= (ushort)~DnsHeaderFlag.RecursionAvailable;
                _flags |= (ushort)value;
            }
        }

        public int Id { get; set; }

        public DnsOpCode OpCode
        {
            get => (DnsOpCode)((DnsHeader.OpCodeMask & _flags) >> DnsHeader.OpCodeShift);
            set
            {
                _flags &= (ushort)~DnsHeader.OpCodeMask;
                _flags |= (ushort)(((ushort)value << DnsHeader.OpCodeShift) & DnsHeader.OpCodeMask);
            }
        }

        public ushort RCode
        {
            get => (ushort)(DnsHeader.RCodeMask & _flags);
            set
            {
                _flags &= (ushort)~DnsHeader.RCodeMask;
                _flags |= (ushort)(value & DnsHeader.RCodeMask);
            }
        }

        public bool UseRecursion
        {
            get => HeaderFlags.HasFlag(DnsHeaderFlag.RecursionDesired);
            set
            {
                if (value)
                {
                    _flags |= (ushort)DnsHeaderFlag.RecursionDesired;
                }
                else
                {
                    _flags &= (ushort)~DnsHeaderFlag.RecursionDesired;
                }
            }
        }

        public DnsRequestHeader(int id, DnsOpCode queryKind)
            : this(id, true, queryKind)
        {
        }

        public DnsRequestHeader(int id, bool useRecursion, DnsOpCode queryKind)
        {
            Id = id;
            OpCode = queryKind;
            UseRecursion = useRecursion;
        }

        public override string ToString()
        {
            return $"{Id} - Qs: {1} Recursion: {UseRecursion} OpCode: {OpCode}";
        }
    }
}