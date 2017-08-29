using System;

namespace HangfireContext
{
    public interface IContext
    {
        bool TryGet<T>(out T value);

        TResult With<TInterface, TClass, TResult>(TClass context, Func<TResult> perform)
            where TClass : TInterface;        
    }
}
