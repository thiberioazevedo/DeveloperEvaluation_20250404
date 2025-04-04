namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class SaleItemResponse
{
    public Guid ProductId { get; set; }
    public Guid SaleId { get; set; }
    public int Quantity { get; set; }
}