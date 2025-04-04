using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Handlers;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class SaleCreatedEventHandler : IHandleMessages<SaleCreatedEvent>
    {
        public Task Handle(SaleCreatedEvent message)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            string json = JsonSerializer.Serialize(message, options);

            Console.WriteLine(json);

            return Task.CompletedTask;
        }
    }
}
