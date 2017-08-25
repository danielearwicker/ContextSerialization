using Hangfire;
using System;
using System.Linq.Expressions;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.States;

namespace Whiffy
{
    public class DirectSubmitter : IBackgroundJobClient
    {
        readonly object _instance;

        public DirectSubmitter(object instance)
        {
            _instance = instance;
        }

        public bool ChangeState([NotNull] string jobId, [NotNull] IState state, [CanBeNull] string expectedState)
        {
            throw new NotImplementedException();
        }

        public string Create([NotNull] Job job, [NotNull] IState state)
        {
            throw new NotImplementedException();
        }

        public void Submit<T>(Expression<Action<T>> job)
        {
            var action = job.Compile();
            action((T)_instance);
        }
    }
}
