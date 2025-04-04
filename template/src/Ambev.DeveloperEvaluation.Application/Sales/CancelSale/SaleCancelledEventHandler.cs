using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Handlers;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale
{
    public class SaleCancelledEventHandler : IHandleMessages<SaleCancelledEvent>
    {
        public Task Handle(SaleCancelledEvent message)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            string json = JsonSerializer.Serialize(message, options);

            Console.WriteLine(json);

            return Task.CompletedTask;
        }
    }
}
