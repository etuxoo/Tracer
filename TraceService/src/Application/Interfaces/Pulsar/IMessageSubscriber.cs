using System;
using System.Threading.Tasks;

namespace TraceService.Infrastructure.Interfaces.Pulsar
{
    public interface IMessageSubscriber<T> where T : class
    {
        public Task SubscribeAsync(string topic, Action<T> handler);
    }
}
