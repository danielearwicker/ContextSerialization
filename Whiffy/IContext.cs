namespace Whiffy
{
    public interface IContext<T>
    {
        T Context { get; }
    }
}
