
namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleItemDeletedEvent
    {
        public Guid Id { get; set; }
        public List<Guid> ProductIdList { get; set; }
    }
}
