namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

public class SaleResponse
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public DateTime Date { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public bool Cancelled { get; set; }
    public decimal Discount { get; set; }
    public decimal PercentageDiscount { get; set; }
    public decimal GrossTotal { get; set; }
    public decimal Total { get; set; }
    public BranchResponse Branch { get; set; }
    public CustomerResponse Customer { get; set; }
}
