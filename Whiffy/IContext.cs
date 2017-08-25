using System;

namespace Whiffy
{
    public interface IContext
    {
        bool TryGet<T>(out T value);

        R With<T, R>(T context, Func<R> perform);

        string AsJson { get; }

        R WithJson<R>(string json, Func<R> perform);
    }
}
