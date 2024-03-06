using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TraceService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace TraceService.Application.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly Stopwatch _timer;

        public PerformanceBehaviour()
        {
            this._timer = new();
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            this._timer.Start();
            TResponse response = await next();
            this._timer.Stop();
            return response;
        }
    }
}
