using Contracts.Enums;
using TraceService.Infrastructure.Pulsar;

namespace Client
{
    public  class TracerPublisher(string brokerUri, string brokerPrefix, string producer)
    {
        private readonly PulsarMessagePublisher _messagePublisher = new(brokerUri, brokerPrefix, producer);

        public async void SendTrace<T>(T model, BancontactTopics topic) where T : class
        {
            await this._messagePublisher.PublishAsync($"{topic}", model);
        }
    }
}
