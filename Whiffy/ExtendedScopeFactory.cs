using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Whiffy
{
    public class ExtendedScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceScopeFactory _impl;
        private readonly IDictionary<Type, object> _instances;

        private class ExtendedScope : IServiceScope, IServiceProvider
        {
            private readonly IServiceScope _impl;
            private readonly IDictionary<Type, object> _instances;

            public ExtendedScope(IServiceScope impl, IDictionary<Type, object> instances)
            {
                _impl = impl;
                _instances = instances;
            }

            public IServiceProvider ServiceProvider => this;

            public void Dispose()
            {
                _impl.Dispose();
            }

            public object GetService(Type serviceType)
            {
                return _instances.TryGetValue(serviceType, out var instance)
                    ? instance
                    : _impl.ServiceProvider.GetService(serviceType);
            }
        }

        public ExtendedScopeFactory(IServiceScopeFactory impl,
                                    IEnumerable<(Type serviceType, object instance)> instances)
        {
            _impl = impl;
            _instances = instances.ToDictionary(i => i.serviceType, i => i.instance);
        }

        public ExtendedScopeFactory(IServiceScopeFactory impl,
                                    params (Type serviceType, object instance)[] instances)
            : this(impl, instances.AsEnumerable()) { }

        public IServiceScope CreateScope()
        {
            return new ExtendedScope(_impl.CreateScope(), _instances);
        }
    }
}
