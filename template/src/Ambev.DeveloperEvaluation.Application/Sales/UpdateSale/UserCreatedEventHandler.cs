using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Handlers;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UserCreatedEventHandler : IHandleMessages<UserCreatedEvent>
    {
        public Task Handle(UserCreatedEvent message)
        {
            var options = new JsonSerializerOptions { WriteIndented = true};

            string json = JsonSerializer.Serialize(message, options);
            
            Console.WriteLine(json);

            return Task.CompletedTask;
        }
    }
}
