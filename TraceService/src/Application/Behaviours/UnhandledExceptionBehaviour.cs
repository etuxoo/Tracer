using System;
using System.Threading;
using System.Threading.Tasks;
using CorrelationId.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TraceService.Application.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger, ICorrelationContextAccessor correlationContextAccessor)
        {
            this._logger = logger;
            this._correlationContextAccessor = correlationContextAccessor;
        }

       

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                string requestName = typeof(TRequest).Name;
                string correlationId = this._correlationContextAccessor.CorrelationContext?.CorrelationId;

                this._logger.LogError(ex,
                    $"[{correlationId}] TraceService Request: Unhandled Exception {ex.Message} for Request {requestName} {request}");

                throw;
            }
        }
    }
}
