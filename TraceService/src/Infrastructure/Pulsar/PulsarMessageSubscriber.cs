using System;
using System.Buffers;
using System.Reflection;
using System.Threading.Tasks;
using DotPulsar.Abstractions;
using DotPulsar;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TraceService.Application.Interfaces.Pulsar;
using DotPulsar.Extensions;
using TraceService.Infrastructure.Interfaces.Pulsar;
using FluentValidation;

namespace TraceService.Infrastructure.Pulsar
{
    public class PulsarMessageSubscriber<T> : IMessageSubscriber<T> where T : class
    {
        private readonly ISerializer _serializer;
        private readonly PulsarOptions _options;
        private readonly ILogger<PulsarMessageSubscriber<T>> _logger;
        //private readonly IValidator<T> _validator;

        private readonly IPulsarClient _client;
        private readonly string _consumerName;

        public PulsarMessageSubscriber(ISerializer serializer, IOptions<PulsarOptions> options, ILogger<PulsarMessageSubscriber<T>> logger)//, IValidator<T> validator)
        {
            this._serializer = serializer;
            this._options = options.Value;
            this._logger = logger;
            //this._validator = validator;

            this._client = PulsarClient.Builder()
                .ServiceUrl(new Uri(this._options.ServiceUrl ?? ""))
                .Build();

            this._consumerName = Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty;
        }

        public async Task SubscribeAsync(string topic, Action<T> handler)
        {
            string topicName = this._options.TopicPrefix + topic;
            string subscriptionName = $"{this._consumerName}-{topic}";
            IConsumer<ReadOnlySequence<byte>> consumer = this._client.NewConsumer().SubscriptionName(subscriptionName).Topic(topicName).Create();

            await foreach (IMessage<ReadOnlySequence<byte>> message in consumer.Messages())
            {
                this._logger.LogInformation("Received a Message with ID: {MessageId}", message.MessageId);
                T deserializedMessage = this._serializer.DeserializeBytes<T>(message.Data.FirstSpan.ToArray());
                try
                {
                    if (deserializedMessage != null)
                    {
                        this.LogMessage(deserializedMessage, message);

                        //await this._validator.ValidateAndThrowAsync(deserializedMessage);

                        handler(deserializedMessage);
                    }
                }
                catch (Exception e)
                {
                    this._logger.LogError("Validation failed : {e}", e);
                    this._logger.LogError("Message with ID: {MessageId} not processed", message.MessageId);
                    continue;
                }

                this._logger.LogInformation("Message with ID: {MessageId} is processed", message.MessageId);
                await consumer.Acknowledge(message);
            }
        }

        private void LogMessage(T deserializedMessage, IMessage<ReadOnlySequence<byte>> message)
        {
            string jsonMessage = this._serializer.Serialize(deserializedMessage);
            this._logger.LogInformation("Message with ID: {MessageId} deserialized successfully. Body: {jsonMessage}",
                message.MessageId, jsonMessage);
        }
    }
}
