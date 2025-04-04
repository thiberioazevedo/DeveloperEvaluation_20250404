using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "You must apply a zero percent discount when then more than four items identical")]
    public void MustApplyZeroPercentDiscountWhenMoreThanFourItemsIdentical()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var sale = new Sale
        {
            //SaleItemCollection = new List<SaleItem>
            //{
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 100 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 200 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 300 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 400 },
                //new SaleItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100 },
            //}
        };

        // Act
        sale.Calculate();

        // Assert
        Assert.Equal(0, sale.PercentageDiscount);
        Assert.Equal(0, sale.Discount);
        Assert.Equal(1100, sale.Total);
        Assert.Equal(1100, sale.GrossTotal);
    }

    [Fact(DisplayName = "Must apply ten percent discount when there are more than four items identical")]
    public void MustApplyTenPercentDiscountWhenMoreThanFourItemsIdentical()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var sale = new Sale
        {
            //SaleItemCollection = new List<SaleItem>
            //{
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 100 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 200 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 300 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 400 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 500 }
            //}
        };

        // Act
        sale.Calculate();

        // Assert
        Assert.Equal(10, sale.PercentageDiscount);
        Assert.Equal(150, sale.Discount);
        Assert.Equal(1350, sale.Total);
        Assert.Equal(1500, sale.GrossTotal);
    }

    [Fact(DisplayName = "Must apply twenty percent discount when there are more than ten items identical")]
    public void MustApplyTwentyPercentDiscountWhenMoreThanTenItemsIdentical()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var sale = new Sale
        {
            //SaleItemCollection = new List<SaleItem>
            //{
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 100 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 200 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 300 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 400 },
                //new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 500 },
                //new SaleItem { ProductId = productId, Quantity = 5, UnitPrice = 100 },
            //}
        };

        // Act
        sale.Calculate();

        // Assert
        Assert.Equal(20, sale.PercentageDiscount);
        Assert.Equal(400, sale.Discount);
        Assert.Equal(1600, sale.Total);
        Assert.Equal(2000, sale.GrossTotal);
    }
}
