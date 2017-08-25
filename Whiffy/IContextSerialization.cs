using System;

namespace Whiffy
{
    public interface IContextSerialization
    {
        string Context { get; }

        R With<R>(string context, Func<R> perform);
    }
}
