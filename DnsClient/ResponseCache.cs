﻿namespace DnsClient
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal class ResponseCache
    {
        private static readonly TimeSpan SInfiniteTimeout = Timeout.InfiniteTimeSpan;

        // max is 24 days
        private static readonly TimeSpan SMaxTimeout = TimeSpan.FromMilliseconds(int.MaxValue);

        private static readonly int SCleanupInterval = (int)TimeSpan.FromMinutes(10).TotalMilliseconds;
        private readonly ConcurrentDictionary<string, ResponseEntry> _cache = new ConcurrentDictionary<string, ResponseEntry>();
        private readonly object _cleanupLock = new object();
        private bool _cleanupRunning;
        private int _lastCleanup;
        private TimeSpan? _minimumTimeout;

        public int Count => _cache.Count;

        public bool Enabled { get; set; }

        public TimeSpan? MinimumTimout
        {
            get => _minimumTimeout;
            set
            {
                if (value.HasValue &&
                    (value < TimeSpan.Zero || value > SMaxTimeout) && value != SInfiniteTimeout)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _minimumTimeout = value;
            }
        }

        public ResponseCache(bool enabled = true, TimeSpan? minimumTimout = null)
        {
            Enabled = enabled;
            MinimumTimout = minimumTimout;
        }

        public static string GetCacheKey(DnsQuestion question, NameServer server)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            return string.Concat(server.Endpoint.Address.ToString(), "#", server.Endpoint.Port.ToString(), "_", question.QueryName.Value, ":", (short)question.QuestionClass, ":", (short)question.QuestionType);
        }

        public IDnsQueryResponse Get(string key)
        {
            return Get(key, out _);
        }

        public IDnsQueryResponse Get(string key, out double? effectiveTtl)
        {
            effectiveTtl = null;
            if (key == null) throw new ArgumentNullException(key);
            if (!Enabled) return null;

            if (_cache.TryGetValue(key, out var entry))
            {
                effectiveTtl = entry.Ttl;
                if (entry.IsExpiredFor(DateTimeOffset.UtcNow))
                {
                    _cache.TryRemove(key, out entry);
                }
                else
                {
                    StartCleanup();
                    return entry.GetResponse();
                }
            }

            return null;
        }

        public bool Add(string key, IDnsQueryResponse response)
        {
            if (key == null) throw new ArgumentNullException(key);
            if (Enabled && response != null && !response.HasError)
            {
                var all = response.AllRecords.ToList();
                if (all.Any())
                {
                    // in millis
                    var minTtl = all.Min(p => p.TimeToLive) * 1000d;

                    if (MinimumTimout == Timeout.InfiniteTimeSpan)
                    {
                        minTtl = SMaxTimeout.TotalMilliseconds;
                    }
                    else if (MinimumTimout.HasValue && minTtl < MinimumTimout.Value.TotalMilliseconds)
                    {
                        minTtl = (long)MinimumTimout.Value.TotalMilliseconds;
                    }

                    if (minTtl < 1d)
                    {
                        return false;
                    }

                    var newEntry = new ResponseEntry(response, minTtl);

                    StartCleanup();
                    return _cache.TryAdd(key, newEntry);
                }
            }

            StartCleanup();
            return false;
        }

        private static void DoCleanup(ResponseCache cache)
        {
            cache._cleanupRunning = true;

            var now = DateTimeOffset.UtcNow;
            foreach (var entry in cache._cache)
            {
                if (entry.Value.IsExpiredFor(now))
                {
                    cache._cache.TryRemove(entry.Key, out _);
                }
            }

            cache._cleanupRunning = false;
        }

        private void StartCleanup()
        {
            if (!Enabled)
            {
                return;
            }

            // TickCount jump every 25days to int.MinValue, adjusting...
            var currentTicks = Environment.TickCount & int.MaxValue;
            if (_lastCleanup + SCleanupInterval < 0 || currentTicks + SCleanupInterval < 0) _lastCleanup = 0;
            if (!_cleanupRunning && _lastCleanup + SCleanupInterval < currentTicks)
            {
                lock (_cleanupLock)
                {
                    if (!_cleanupRunning && _lastCleanup + SCleanupInterval < currentTicks)
                    {
                        _lastCleanup = currentTicks;

                        Task.Factory.StartNew(
                            state => DoCleanup((ResponseCache)state),
                            this,
                            CancellationToken.None,
                            TaskCreationOptions.DenyChildAttach,
                            TaskScheduler.Default);
                    }
                }
            }
        }

        private class ResponseEntry
        {
            private readonly IDnsQueryResponse _response;

            public bool IsExpiredFor(DateTimeOffset forDate) => forDate >= ExpiresAt;

            private DateTimeOffset ExpiresAt { get; }

            private DateTimeOffset Created { get; }

            public double Ttl { get; }

            // returns in seconds, not MS!
            private int Elapsed(DateTimeOffset? since = null)
            {
                if (since == null)
                {
                    since = DateTimeOffset.UtcNow;
                }

                var elapsedMillis = (int)(since.Value - Created).TotalMilliseconds;
                if (elapsedMillis < 0)
                {
                    return 0;
                }

                return elapsedMillis / 1000;
            }

            public IDnsQueryResponse GetResponse()
            {
                var elapsed = Elapsed();
                if (elapsed <= 0)
                {
                    return _response;
                }

                var response = new DnsResponseMessage(_response.Header, _response.MessageSize)
                {
                    Audit = (_response as DnsQueryResponse)?.Audit ?? new LookupClientAudit()
                };

                foreach (var record in _response.Questions)
                {
                    response.AddQuestion(record);
                }

                foreach (var record in _response.Answers)
                {
                    var clone = record.Clone();
                    clone.TimeToLive = clone.TimeToLive - elapsed;
                    response.AddAnswer(clone);
                }

                foreach (var record in _response.Additionals)
                {
                    var clone = record.Clone();
                    clone.TimeToLive = clone.TimeToLive - elapsed;
                    response.AddAnswer(clone);
                }

                foreach (var record in _response.Authorities)
                {
                    var clone = record.Clone();
                    clone.TimeToLive = clone.TimeToLive - elapsed;
                    response.AddAnswer(clone);
                }

                var qr = response.AsQueryResponse(_response.NameServer);

                return qr;
            }

            public ResponseEntry(IDnsQueryResponse response, double ttlInMs)
            {
                Debug.Assert(response != null);
                Debug.Assert(ttlInMs >= 0);

                _response = response;
                Ttl = ttlInMs;
                Created = DateTimeOffset.UtcNow;
                ExpiresAt = Created.AddMilliseconds(Ttl);
            }
        }
    }
}