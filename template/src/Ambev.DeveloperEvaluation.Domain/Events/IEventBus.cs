namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public interface IEventBus
    {
        Task SendAsync<T>(T @event) where T : class;
    }
}
