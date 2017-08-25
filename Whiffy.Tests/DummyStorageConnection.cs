using System;
using Hangfire.Common;
using Hangfire.Storage;
using Hangfire.Server;
using System.Collections.Generic;
using Hangfire.Annotations;
using System.Threading;
using Hangfire;

namespace Whiffy
{
    public class DummyJobStorage : JobStorage
    {
        public override IStorageConnection GetConnection()
        {
            return new DummyStorageConnection();
        }

        public override IMonitoringApi GetMonitoringApi()
        {
            throw new NotImplementedException();
        }
    }

    public class DummyStorageConnection : IStorageConnection
    {
        public IDisposable AcquireDistributedLock(string resource, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void AnnounceServer(string serverId, ServerContext context)
        {
            throw new NotImplementedException();
        }

        public string CreateExpiredJob(Job job, IDictionary<string, string> parameters, DateTime createdAt, TimeSpan expireIn)
        {
            throw new NotImplementedException();
        }

        public IWriteOnlyTransaction CreateWriteTransaction()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IFetchedJob FetchNextJob(string[] queues, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetAllEntriesFromHash([NotNull] string key)
        {
            throw new NotImplementedException();
        }

        public HashSet<string> GetAllItemsFromSet([NotNull] string key)
        {
            throw new NotImplementedException();
        }

        public string GetFirstByLowestScoreFromSet(string key, double fromScore, double toScore)
        {
            throw new NotImplementedException();
        }

        public JobData GetJobData([NotNull] string jobId)
        {
            throw new NotImplementedException();
        }

        public string GetJobParameter(string id, string name)
        {
            throw new NotImplementedException();
        }

        public StateData GetStateData([NotNull] string jobId)
        {
            throw new NotImplementedException();
        }

        public void Heartbeat(string serverId)
        {
            throw new NotImplementedException();
        }

        public void RemoveServer(string serverId)
        {
            throw new NotImplementedException();
        }

        public int RemoveTimedOutServers(TimeSpan timeOut)
        {
            throw new NotImplementedException();
        }

        public void SetJobParameter(string id, string name, string value)
        {
            throw new NotImplementedException();
        }

        public void SetRangeInHash([NotNull] string key, [NotNull] IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            throw new NotImplementedException();
        }
    }
}
