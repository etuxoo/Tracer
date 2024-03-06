using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using TraceService.Infrastructure.Interfaces.Pulsar;
using MediatR;
using TraceService.Domain.Entities.Bancontact;
using TraceService.Application.Features.Tracing.Commands.Bancontact;

namespace TraceService.Infrastructure.Subscribers
{
    public class TraceSubscriber<T>(IMessageSubscriber<T> messageSubscriber, ISender mediator) : BackgroundService where T : class
    {
        private readonly IMessageSubscriber<T> _messageSubscriber = messageSubscriber;
        private readonly ISender _mediator = mediator;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this._messageSubscriber.SubscribeAsync(topic: $"{typeof(T).Name[..^7]}",
    message => this._mediator.Send(message));
            }
            catch (System.Exception)
            {
                throw;
            }


            return Task.CompletedTask;
        }
    }
}
