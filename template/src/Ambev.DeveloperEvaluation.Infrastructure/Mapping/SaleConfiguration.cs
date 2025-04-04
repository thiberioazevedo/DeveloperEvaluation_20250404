using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.Infrastructure.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(u => u.Id).HasName("PK_Sales");

        builder.Property(u => u.Id);
        builder.Property(u => u.Number).IsRequired().ValueGeneratedOnAdd();
        builder.Property(u => u.Date).IsRequired().HasConversion( v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        builder.Property(u => u.CustomerId).IsRequired();
        builder.Property(u => u.BranchId).IsRequired();
        builder.Property(u => u.Cancelled).IsRequired();
        builder.Property(u => u.Discount).IsRequired();
        builder.Property(u => u.PercentageDiscount).IsRequired();
        builder.Property(u => u.GrossTotal).IsRequired();
        builder.Property(u => u.Total).IsRequired();

        builder.HasIndex(u => u.Number).IsUnique().HasDatabaseName("UN_Sales_Number");

        builder.HasOne(u => u.Branch).WithMany(u => u.SaleCollection).HasConstraintName("FK_Sales_Branchs_BranchId");
        builder.HasOne(u => u.Customer).WithMany(u => u.SaleCollection).HasConstraintName("FK_Sales_Customers_CustomerId");
    }
}
