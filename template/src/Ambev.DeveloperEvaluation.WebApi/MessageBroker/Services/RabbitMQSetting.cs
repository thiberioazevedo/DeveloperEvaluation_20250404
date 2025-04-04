namespace Ambev.DeveloperEvaluation.WebApi.MessageBroker.Services
{
    public class RabbitMQSetting
    {
        public string? HostName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    public static class RabbitMQQueues
    {
        public const string CreateCDBQueue = "CreateCDBQueue";
    }
}
