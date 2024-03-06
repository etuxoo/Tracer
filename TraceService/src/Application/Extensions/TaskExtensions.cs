using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TraceService.Application.Extensions
{
    public static class TaskExtensions
    {
        public static void Forget(this Task task, ILogger logger)
        {
            task.ContinueWith(
                t => logger.LogError(t.Exception.Message),
                TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
