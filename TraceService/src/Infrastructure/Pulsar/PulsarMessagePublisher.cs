using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using TraceService.Application.Interfaces.Pulsar;


namespace TraceService.Infrastructure.Pulsar
{
    public class PulsarMessagePublisher : IMessagePublisher
    {
        private readonly IPulsarClient _client;
        private readonly string _producerName;

        private readonly string _pulsarUri;
        private readonly string _pulsarPrefix;

        private readonly PulsarSerializer _serializer;

        private readonly ConcurrentDictionary<string, IProducer<ReadOnlySequence<byte>>> _producers = new();

        public PulsarMessagePublisher(string Uri, string prefix, string producer)
        {
            this._pulsarUri = Uri;
            this._pulsarPrefix = prefix;

            this._client = PulsarClient.Builder()
                .ServiceUrl(new Uri(this._pulsarUri))
                .Build();

            this._producerName = producer;
            this._serializer = new PulsarSerializer();
        }

        public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class
        {
            IProducer<ReadOnlySequence<byte>> producer = this._producers.GetOrAdd(topic, this._client.NewProducer()
                                                 .ProducerName(this._producerName)
                                                 .Topic($"{this._pulsarPrefix}{topic}")
                                                 .Create());

            byte[] data = this._serializer.SerializeBytes(message);

            await producer.Send(data, cancellationToken);
        }
    }
}
