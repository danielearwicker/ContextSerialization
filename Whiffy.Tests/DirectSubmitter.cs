using Hangfire;
using System;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Server;

namespace Whiffy
{
    public class DirectSubmitter : IBackgroundJobClient
    {
        private IBackgroundJobPerformer _performer;

        public DirectSubmitter(IBackgroundJobPerformer performer)
        {
            _performer = performer;
        }

        public bool ChangeState([NotNull] string jobId, [NotNull] IState state, [CanBeNull] string expectedState)
        {
            throw new NotImplementedException();
        }

        public string Create([NotNull] Job job, [NotNull] IState state)
        {
            var backgroundJob = new BackgroundJob(string.Empty, job, DateTime.MinValue);

            using (var storage = JobStorage.Current.GetConnection())
            {
                var performContext = new PerformContext(storage, backgroundJob, new DummyCancellation());
                _performer.Perform(performContext);
            }
            
            return Guid.NewGuid().ToString();
        }
    }
}
