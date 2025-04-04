using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.Infrastructure.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(u => u.Id).HasName("PK_Products");

        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.Name).HasMaxLength(50).IsRequired();
        builder.Property(u => u.UnitPrice).IsRequired();
    }
}
