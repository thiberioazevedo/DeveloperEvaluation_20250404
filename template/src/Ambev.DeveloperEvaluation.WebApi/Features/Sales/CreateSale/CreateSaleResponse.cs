namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleResponse
{
    public Guid Id { get; set; }
    public virtual Guid CustomerId { get; set; }
    public virtual Guid BranchId { get; set; }
}
