using System.Threading;
using System.Threading.Tasks;

namespace TraceService.Application.Interfaces.Pulsar
{
    public interface IMessagePublisher
    {
        public Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class;
    }
}
