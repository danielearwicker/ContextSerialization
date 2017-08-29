using System;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;

namespace HangfireContext
{
    public static class ContextExtensions
    {
        public static T GetRequired<T>(this IContext context)
        {
            if (context.TryGet<T>(out var value))
            {
                return value;
            }

            throw new NotSupportedException($"No contextual object of type {typeof(T)} is available");
        }

        public static void With<TInterface, TClass>(this IContext context, 
                                                    TClass implementation, 
                                                    Action perform)
            where TClass : TInterface
        {
            context.With<TInterface, TClass, int>(implementation, () => { perform(); return 0; });
        }

        public static void AddHangfireContext(this IServiceCollection services)
        {
            var context = new ContextManager();
            services.AddSingleton<IContext>(context);
            GlobalJobFilters.Filters.Add(new ContextFilter(context));
        }
    }
}
