using Hangfire;

namespace HangfireContext.Stress
{
    public class IntermediateJob
    {
        IBackgroundJobClient _hangfire;

        public IntermediateJob(IBackgroundJobClient client)
        {
            _hangfire = client;
        }

        public void Execute(int level, int secretCode)
        {
            if (level == 0)
            {
                _hangfire.Enqueue<EndJob>(job => job.Execute(secretCode));
            }
            else
            {
                _hangfire.Enqueue<IntermediateJob>(job => job.Execute(level - 1, secretCode));
            }
        }
    }
}
