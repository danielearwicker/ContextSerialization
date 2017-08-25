using System;
using System.Linq.Expressions;

namespace Whiffy
{
    public class DirectSubmitter : IJobSubmitter
    {
        readonly object _instance;

        public DirectSubmitter(object instance)
        {
            _instance = instance;
        }

        public void Submit<T>(Expression<Action<T>> job)
        {
            var action = job.Compile();
            action((T)_instance);
        }
    }
}
