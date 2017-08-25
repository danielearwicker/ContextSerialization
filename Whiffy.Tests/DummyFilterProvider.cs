using Hangfire.Common;
using System.Collections.Generic;

namespace Whiffy
{
    public class DummyFilterProvider : IJobFilterProvider
    {
        public IEnumerable<JobFilter> GetFilters(Job job)
        {
            yield break;
        }
    }
}
