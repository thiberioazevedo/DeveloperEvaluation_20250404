using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public SaleItem()
    {
    }

    public SaleItem(Guid productId, Guid saleId, int quantity)
    {
        ProductId = productId;
        SaleId = saleId;
        Quantity = quantity;
    }

    public Guid ProductId { get; private set; }
    public virtual Product Product { get; private set; }
    public Guid SaleId { get; private set; }
    public virtual Sale Sale { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalAmount => (Quantity * UnitPrice);

    public override ValidationResult GetValidationResult()
    {
        return new SaleItemValidator().Validate(this);
    }

    public void SetQuantity(int quantity)
    {
        this.Quantity = quantity;
    }

    public bool SetUnitPrice(decimal unitPrice)
    {
        if (UnitPrice > 0)
            return false;

        UnitPrice = unitPrice;

        return true;
    }

    internal bool SetSaleId(Guid id)
    {
        if (SaleId != Guid.Empty)
            return false;

        SaleId = id;
        return true;
    }
}