namespace HangfireContext
{
    public interface IContextSerialization
    {
        string Peek();

        void Push(string state);

        void Pop();
    }
}
