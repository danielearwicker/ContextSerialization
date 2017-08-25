using System;
using System.Linq.Expressions;
using Hangfire.Common;
using Hangfire.Storage;
using Hangfire;

namespace Whiffy
{
    public class ContextSubmitter : IContextSubmitter
    {
        IContextSerialization _context;
        IBackgroundJobClient _client;
        
        public ContextSubmitter(IContextSerialization context, IBackgroundJobClient client)
        {
            _context = context;
            _client = client;
        }

        public void Submit<T>(Expression<Action<T>> expr)
        {
            var job = Job.FromExpression(expr);
            var data = InvocationData.Serialize(job);

            _client.Submit<ContextJobExecutor>(p => p.Execute(
                _magicNumber,
                data.Type,
                data.Method,
                data.ParameterTypes,
                data.Arguments,
                null));
        }
    }
}
