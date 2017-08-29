using System;
using Microsoft.AspNetCore.Mvc;
using Hangfire;

namespace HangfireContext.Stress.Controllers
{
    [Route("api/[controller]")]
    public class DemoController : Controller
    {
        IContext _context;
        IBackgroundJobClient _hangfire;

        Random _random = new Random();

		public DemoController(IContext context, IBackgroundJobClient hangfire)
        {
            _context = context;
            _hangfire = hangfire;       
        }

        [HttpPost]
        public void Post()
        {
            for (var n = 0; n < 10000; n++)
            {
                var secrets = new Secrets { Code = _random.Next(100) };

                _context.With<ISecrets, Secrets>(secrets, () =>
                {
                    _hangfire.Enqueue<IntermediateJob>(job => job.Execute(_random.Next(0, 100), secrets.Code));
                });
            }
        }
    }
}
