namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCancelledEvent
    {
        public Guid Id { get; internal set; }

        public SaleCancelledEvent(Guid id)
        {
            Id = id;
        }
    }
}
