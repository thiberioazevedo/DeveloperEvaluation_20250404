namespace Ambev.DeveloperEvaluation.Common.MessageBroker.Services
{
    public interface IMessageBrokerPublisher
    {
        Task PublishMessageAsync<T>(T message, string queueName);
    }
}
