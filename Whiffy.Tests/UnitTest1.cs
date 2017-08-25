using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Whiffy.Tests
{
    public interface IBakery
    {
        void Biscuits(int howMany, bool chocolate);
    }

    public class Bakery : IBakery
    {
        readonly IMagicNumber _magicNumber;

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
        [Fact]
        public void Test1()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IBakery, Bakery>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var scopeFactory = serviceProvider.GetService<IServiceScopeFactory>();

            var proxy = new MagicProxy(new DummyFilterProvider(), scopeFactory);

            var direct = new DirectSubmitter(proxy);

            new MagicSubmitter(direct, 52).Submit<Bakery>(b => b.Biscuits(100, true));
        }
    }
}
