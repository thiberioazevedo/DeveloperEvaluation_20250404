using System.Text;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Common.MessageBroker.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Ambev.DeveloperEvaluation.WebApi.MessageBroker.Services
{
    public class RabbitMQPublisher : IMessageBrokerPublisher
    {
        private readonly RabbitMQSetting _rabbitMqSetting;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public RabbitMQPublisher(IOptions<RabbitMQSetting> rabbitMqSetting)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;
        }

        public async Task PublishMessageAsync<T>(T message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting?.HostName ?? string.Empty,
                UserName = _rabbitMqSetting?.UserName ?? string.Empty,
                Password = _rabbitMqSetting?.Password ?? string.Empty
            };

            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var messageJson = JsonSerializer.Serialize(message, _jsonOptions);

            var body = Encoding.UTF8.GetBytes(messageJson);

            await Task.Run(() => channel.BasicPublishAsync("", queueName, body));
        }
    }
}
