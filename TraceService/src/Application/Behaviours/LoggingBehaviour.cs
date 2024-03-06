using System.Threading;
using System.Threading.Tasks;
using CorrelationId.Abstractions;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace TraceService.Application.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public LoggingBehaviour(ILogger<TRequest> logger, ICorrelationContextAccessor correlationContextAccessor)
        {
            this._logger = logger;
            this._correlationContextAccessor = correlationContextAccessor;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            string userName = string.Empty;
            string correlationId = this._correlationContextAccessor.CorrelationContext?.CorrelationId;
            this._logger.LogInformation($"[{correlationId}] TraceService Request: {requestName} {request}");
            await Task.CompletedTask;
        }
    }
}
