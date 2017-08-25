using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Hangfire;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Hangfire.Common;
using Hangfire.Server;

namespace Whiffy.Tests
{
    public interface IBakery
    {
        void Biscuits(int howMany, bool chocolate);
    }

    public class Bakery : IBakery
    {
        IContext

        public Bakery(IMagicNumber magicNumber)
        {
            _magicNumber = magicNumber;
        }

        public void Biscuits(int howMany, bool chocolate)
        {
            
        }
    }

    public class UnitTest1
    {
        static UnitTest1()
        {
            GlobalConfiguration.Configuration.UseStorage(new DummyJobStorage());
        }

        [Fact]
        public void Test1()
        {
            var services = new ServiceCollection();

            // Same as in standard UseHangfire(...)
            services.TryAddSingleton<IJobFilterProvider>(_ => GlobalJobFilters.Filters);
            services.TryAddSingleton<IBackgroundJobPerformer>(x => new BackgroundJobPerformer(
                x.GetRequiredService<IJobFilterProvider>(),
                x.GetRequiredService<JobActivator>()));

            // Fake - just executes the job synchronously
            services.AddTransient<IBackgroundJobClient, DirectSubmitter>();

            services.AddTransient<IBakery, Bakery>();



            var serviceProvider = services.BuildServiceProvider();
            
            var direct = new DirectSubmitter(proxy);

            new MagicSubmitter(direct, 52).Submit<Bakery>(b => b.Biscuits(100, true));
        }
    }
}
