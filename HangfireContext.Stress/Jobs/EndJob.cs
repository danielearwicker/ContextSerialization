using System;

namespace HangfireContext.Stress
{
    public class EndJob
    {
        IContext _context;

        public EndJob(IContext context)
        {
            _context = context;
        }

        public void Execute(int secretCode)
        {
            if (_context.GetRequired<ISecrets>().Code != secretCode)
            {
                throw new InvalidOperationException();
            }

            Console.WriteLine(secretCode);
        }
    }
}
