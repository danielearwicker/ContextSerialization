using System;
using System.Linq.Expressions;

namespace Whiffy
{
    public interface IContextSubmitter
    {
        void Submit<T>(Expression<Action<T>> job);
    }
}
