using System;
using Hangfire.Common;
using Hangfire.Storage;
using Hangfire.Server;
using Hangfire;
using System.Threading;

namespace Whiffy
{

    public class DummyCancellation : IJobCancellationToken
    {
        private CancellationTokenSource _source = new CancellationTokenSource();

        public CancellationToken ShutdownToken => _source.Token;

        public void ThrowIfCancellationRequested()
        {
            ShutdownToken.ThrowIfCancellationRequested();
        }
    }

    public class ContextJobExecutor<T>
    {
        private IJobFilterProvider _filters;
        private IBackgroundJobPerformer _performer;

        public ContextJobExecutor(IJobFilterProvider filters,
                          IBackgroundJobPerformer performer)
        {
            _filters = filters;
            _performer = performer;
        }

        public void Execute(T contextData,
                            string type,
                            string method,
                            string paramTypes,
                            string argValues,
                            IJobCancellationToken cancellation)
        {
            var job = new InvocationData(type, method, paramTypes, argValues).Deserialize();

            var backgroundJob = new BackgroundJob(string.Empty, job, DateTime.MinValue);

            using (var storage = JobStorage.Current.GetConnection())
            {
                var performContext = new PerformContext(
                    storage, backgroundJob, cancellation ?? new DummyCancellation());

                _performer.Perform(performContext);
            }
        }
    }
}
