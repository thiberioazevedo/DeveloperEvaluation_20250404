using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.Results;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ambev.DeveloperEvaluation.Domain.Entities;


public class Sale : BaseEntity
{
    public Sale() {
        SaleItemCollection = new List<SaleItem>();
    }
    public Sale(Guid id, Guid branchId, Guid customerId, DateTime date, ICollection<SaleItem>? saleItemCollection = null)
    {
        Id = id;
        BranchId = branchId;
        CustomerId = customerId;
        Date = date;

        if (saleItemCollection != null)
            SaleItemCollection = saleItemCollection;
    }

    public Sale(Guid branchId, Guid customerId, DateTime date)
    {
        BranchId = branchId;
        CustomerId = customerId;
        Date = date;
        Active();
    }

    public DateTime Date { get; internal set; }
    public virtual Guid CustomerId { get; internal set; }
    public virtual Guid BranchId { get; internal set; }
    public bool Cancelled { get; internal set; }
    public decimal Discount { get; internal set; }
    public decimal PercentageDiscount { get; internal set; }
    public decimal GrossTotal { get; internal set; }
    public decimal Total { get; internal set; }
    public virtual Branch Branch { get; internal set; }
    public virtual Customer Customer { get; internal set; }
    public ICollection<SaleItem>? SaleItemCollection { get; internal set; }

    public bool Cancel() {
        if (Cancelled)
            return false;

        Cancelled = true;

        return true;
    }

    public bool Active() {
        if (!Cancelled)
            return false;

        Cancelled = false;

        return true;
    }

    public void Calculate()
    {
        SetGrossTotal();
        SetPercentageDiscount();
        SetDiscount();
        SetTotal();
    }

    public override ValidationResult GetValidationResult()
    {
        return new SaleValidator().Validate(this);
    }

    public bool SetBranchId(Guid branchId) {
        if (BranchId == branchId)
            return false;

        BranchId = branchId;

        return true;
    }
    public bool SetCustomerId(Guid customerId){
        if (CustomerId == customerId)
            return false;

        CustomerId = customerId;

        return true;
    }
    public bool SetDate(DateTime date)
    {
        if (this.Date == date)
            return false;

        Date = date;

        return true;
    }
    public void AddRange(ICollection<SaleItem> saleItemCollection) {
        foreach (var saleItem in saleItemCollection)
        {
            saleItem.SetSaleId(Id);
            SaleItemCollection.Add(saleItem);
        }
    }

    public void RemoveRange(ICollection<SaleItem> saleItemCollection)
    {
        foreach (var saleItem in saleItemCollection){ 
            SaleItemCollection.Remove(saleItem);
        }
    }

    void SetGrossTotal() {
        GrossTotal = SaleItemCollection == null ? 0 : SaleItemCollection.Sum(i => i.Quantity * i.UnitPrice);
    }

    void SetPercentageDiscount() {
        switch (SaleItemCollection == null ? 0 : SaleItemCollection.Sum(i => i.Quantity))
        {
            case int n when (n <= 4):
                PercentageDiscount = 0;
                return;

            case int n when (n < 10):
                PercentageDiscount = 10;
                return;

            default:
                PercentageDiscount = 20;
                return;

        }
    }

    void SetDiscount() {
        Discount = Math.Round(GrossTotal * (PercentageDiscount / (decimal)100), 2);
    }

    void SetTotal() {
        Total = GrossTotal - Discount;
    }
}