using System;
using System.Linq.Expressions;
using Hangfire.Common;
using Hangfire.Storage;

namespace Whiffy
{
    public class MagicSubmitter : IJobSubmitter
    {
        private IJobSubmitter _transport;
        private int _magicNumber;

        public MagicSubmitter(IJobSubmitter transport, int magicNumber)
        {
            _transport = transport;
            _magicNumber = magicNumber;
        }

        public void Submit<T>(Expression<Action<T>> expr)
        {
            var job = Job.FromExpression(expr);
            var data = InvocationData.Serialize(job);

            _transport.Submit<MagicProxy>(p => p.Execute(
                _magicNumber,
                data.Type,
                data.Method,
                data.ParameterTypes,
                data.Arguments,
                null));
        }
    }
}
