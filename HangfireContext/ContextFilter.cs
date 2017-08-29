using System;
using System.Linq.Expressions;
using Hangfire.Common;
using Hangfire.Storage;
using Hangfire;
using Hangfire.Client;
using Hangfire.Server;

namespace HangfireContext
{
    public class ContextFilter : IClientFilter, IServerFilter
    {
        private readonly IContextSerialization _context;

        public ContextFilter(IContextSerialization context)
        {
            _context = context;
        }        
        
        public void OnCreating(CreatingContext filterContext)
        {
            filterContext.SetJobParameter("contextSnapshot", _context.Peek());
        }

        public void OnCreated(CreatedContext filterContext)
        {
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            _context.Push(filterContext.GetJobParameter<string>("contextSnapshot"));
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            _context.Pop();
        }
    }    
}
