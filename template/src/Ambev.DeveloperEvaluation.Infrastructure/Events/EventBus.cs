using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Infrastructure.Events
{
    public class EventBus: IEventBus
    {
        private readonly IBus _bus;

        public EventBus(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendAsync<T>(T @event) where T : class
        {
            await _bus.Send(@event);
        }
    }
}
