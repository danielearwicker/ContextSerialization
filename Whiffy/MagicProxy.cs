using System;
using Hangfire.Common;
using Hangfire.Storage;
using Hangfire.Server;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.AspNetCore;
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

    public class MagicProxy
    {
        private IJobFilterProvider _filters;
        private IServiceScopeFactory _scopes;

        public MagicProxy(IJobFilterProvider filters,
                          IServiceScopeFactory scopes)
        {
            _filters = filters;
            _scopes = scopes;
        }

        public void Execute(int magicNumber,
                            string type,
                            string method,
                            string paramTypes,
                            string argValues,
                            IJobCancellationToken cancellation)
        {
            var job = new InvocationData(type, method, paramTypes, argValues)
                            .Deserialize();

            var backgroundJob = new BackgroundJob(string.Empty, job, DateTime.MinValue);

            var magicNumberInstance = new MagicNumber { Value = magicNumber };

            var extendedScopes = new ExtendedScopeFactory(_scopes,
                    (typeof(IMagicNumber), magicNumberInstance));

            var activator = new AspNetCoreJobActivator(extendedScopes);
            var storage = new DummyStorageConnection();
            var performContext = new PerformContext(storage, 
                                                    backgroundJob, 
                                                    cancellation ?? new DummyCancellation());
            var performer = new BackgroundJobPerformer(_filters, activator);
            performer.Perform(performContext);
        }
    }
}
