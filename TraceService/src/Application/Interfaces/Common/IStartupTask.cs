using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TraceService.Application.Interfaces
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
        void Execute();
    }
}
