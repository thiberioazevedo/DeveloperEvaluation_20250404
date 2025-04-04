using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Infrastructure.Persistence;

public class DefaultContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Sale> Sales { get; set; }

    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        // Seed data for Branches
        var branch1 = new Branch(Guid.Parse("87aa2ceb-a836-4db2-9548-7a07d5a32ab1"), "Branch 1", 1);
        var branch2 = new Branch(Guid.Parse("6c52eef1-a00f-481e-8dc9-d579647dd25d"), "Branch 2", 2);

        modelBuilder.Entity<Branch>().HasData(branch1, branch2);

        // Seed data for Customers
        var customer1 = new Customer(Guid.Parse("6f31923f-d966-4cee-92d1-3a3fd70f8324"), "Customer 1", "customer1@example.com", 1);
        var customer2 = new Customer(Guid.Parse("1007cb6f-d2bd-44d5-ad80-e34c344147c5"), "Customer 2", "customer2@example.com", 2);
        modelBuilder.Entity<Customer>().HasData(customer1, customer2);

        // Seed data for Products
        var product1 = new Product(Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), "Product 1", 10.00m, 1);
        var product2 = new Product(Guid.Parse("b2c3d4e5-f678-90ab-cdef-234567890abc"), "Product 2", 15.00m, 2);

        modelBuilder.Entity<Product>().HasData(product1, product2);

        //// Seed data for Sales
        //var dateTime = new DateTime(2025, 04, 03);
        //var sale1 = new Sale(Guid.Parse("864922a2-6411-4b9f-b603-ee47d70c7668"), branch1.Id, customer1.Id, dateTime);
        //var sale2 = new Sale(Guid.Parse("c2eed86c-9f65-400d-8992-5bf28e83be27"), branch2.Id, customer2.Id, dateTime);

        //// Seed data for SalesItems
        //var salesItem1 = new SaleItem(product1.Id, sale1.Id, 2);
        //var salesItem2 = new SaleItem(product2.Id, sale2.Id, 1);

        //modelBuilder.Entity<Sale>().HasData(sale1, sale2);

        //modelBuilder.Entity<SaleItem>().HasData(salesItem1, salesItem2);
    }
}
public class YourDbContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{
    public DefaultContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DefaultContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
               connectionString,
               //b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.WebApi")
               b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.Infrastructure")
        );

        return new DefaultContext(builder.Options);
    }
}