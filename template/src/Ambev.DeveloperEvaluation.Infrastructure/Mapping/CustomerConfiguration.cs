using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.Infrastructure.Mapping;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(u => u.Id).HasName("PK_Customers");

        builder.HasIndex(u => u.Name).IsUnique().HasDatabaseName("UN_Customers_Name");

        builder.Property(u => u.Id);
        builder.Property(u => u.Name).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Number).ValueGeneratedOnAdd();

        builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("UN_Customers_Email");
    }
}
