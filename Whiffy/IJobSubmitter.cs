using System;
using System.Linq.Expressions;

namespace Whiffy
{
    public interface IContextSubmitter
    {
        string Enqueue<T>(Expression<Action<T>> job);
    }
}
