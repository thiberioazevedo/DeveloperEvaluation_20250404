using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleResult
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
    public BranchResult Branch { get; set; }
    public CustomerResult Customer { get; set; }
    public ICollection<SaleItemResult> SaleItemCollection { get; set; }
}
