namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class SaleItemCommand
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}