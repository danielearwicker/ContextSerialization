using System;
using System.Linq.Expressions;

namespace Whiffy
{
    public interface IJobSubmitter
    {
        void Submit<T>(Expression<Action<T>> job);
    }
}
