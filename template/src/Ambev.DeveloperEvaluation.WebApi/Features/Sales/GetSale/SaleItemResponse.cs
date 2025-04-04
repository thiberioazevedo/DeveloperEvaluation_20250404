namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class SaleItemResponse
{
    public Guid ProductId { get; set; }
    public Guid SaleId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}