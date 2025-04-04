using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.Infrastructure.Mapping;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branchs");

        builder.HasKey(u => u.Id).HasName("PK_Branchs");

        builder.Property(u => u.Id);
        builder.Property(u => u.Name).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Number).ValueGeneratedOnAdd();

        builder.HasIndex(u => u.Name).IsUnique().HasDatabaseName("UN_Branchs_Name");
    }
}
