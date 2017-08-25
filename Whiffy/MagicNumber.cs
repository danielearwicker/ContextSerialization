using System.Reflection;

namespace Whiffy
{
    public interface IMagicNumber
    {
        int Value { get; }
    }

    public class MagicNumber : IMagicNumber
    {
        public int Value { get; set; }
    }
}
