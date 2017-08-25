using System;

namespace Whiffy
{
    public interface IContextInjection<T>
    {
        R With<R>(T context, Func<R> perform);
    }
}
