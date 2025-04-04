namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
public class UpdateSaleResult
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public DateTime Date { get; set; }
    public virtual Guid CustomerId { get; set; }
    public virtual Guid BranchId { get; set; }
    public bool Cancelled { get; set; }
    public decimal Discount { get; set; }
    public decimal PercentageDiscount { get; set; }
    public decimal GrossTotal { get; set; }
    public decimal Total { get; set; }
    public ICollection<SaleItemResult> SaleItemCollection { get; set; }
}
