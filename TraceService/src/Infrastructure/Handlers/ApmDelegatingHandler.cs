using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CorrelationId.Abstractions;

namespace TraceService.Infrastructure.Handlers
{
    public class ApmDelegatingHandler : DelegatingHandler
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public ApmDelegatingHandler(ICorrelationContextAccessor correlationContextAccessor)
        {
            this._correlationContextAccessor = correlationContextAccessor;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var tracingData = (Agent.Tracer.CurrentSpan?.OutgoingDistributedTracingData
            //                ?? Agent.Tracer.CurrentTransaction?.OutgoingDistributedTracingData)?.SerializeToString();

            //request.Headers.Add("traceparent", tracingData);
            request.Headers.Add("X-Correlation-Id", this._correlationContextAccessor.CorrelationContext.CorrelationId);
            return base.Send(request, cancellationToken);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var tracingData = (Agent.Tracer.CurrentSpan?.OutgoingDistributedTracingData
            //    ?? Agent.Tracer.CurrentTransaction?.OutgoingDistributedTracingData)?.SerializeToString();

            //request.Headers.Add("traceparent", tracingData);
            request.Headers.Add("X-Correlation-Id", this._correlationContextAccessor.CorrelationContext.CorrelationId);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
