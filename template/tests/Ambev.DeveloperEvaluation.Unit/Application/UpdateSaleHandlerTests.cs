using Xunit;
using AutoMapper;
using FluentValidation;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Ambev.DeveloperEvaluation.Infrastructure.Repositories;
using Ambev.DeveloperEvaluation.Unit;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = new SaleRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());
        _saleItemRepository = new SaleItemRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());
        _productRepository = new ProductRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());
        _branchRepository = new BranchRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());
        _customerRepository = new CustomerRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());

        _mapper = Substitute.For<IMapper>();
        _eventBus = Substitute.For<IEventBus>();

        _handler = new UpdateSaleHandler(
            _saleRepository,
            _saleItemRepository,
            _productRepository,
            _branchRepository,
            _customerRepository,
            _mapper,
            _eventBus
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenValidationFails()
    {
        var command = new UpdateSaleCommand();
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenBranchNotFound()
    {
        var branch = new Branch();
        branch.SetName("Branch 2");
        await _branchRepository.CreateAsync(branch);

        var customer = new Customer(Guid.NewGuid(), "Customer 2", "customer2@email", 1);
        branch.SetName("Customer 2");
        await _customerRepository.CreateAsync(customer);

        var sale = new Sale(branch.Id, customer.Id, new DateTime(2025, 4, 4));
        await _saleRepository.CreateAsync(sale);

        // Arrange
        var command = new UpdateSaleCommand
        {
            Id = sale.Id,
            CustomerId = customer.Id,
            BranchId = Guid.NewGuid(),
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCustomerNotFound()
    {
        var branch = new Branch();
        branch.SetName("Branch 1");
        await _branchRepository.CreateAsync(branch);

        var customer = new Customer(Guid.NewGuid(), "Customer 1", "customer1@email", 1);
        branch.SetName("Customer 1");
        await _customerRepository.CreateAsync(customer);

        var sale = new Sale(branch.Id, customer.Id, new DateTime(2025, 4, 4));
        await _saleRepository.CreateAsync(sale);

        // Arrange
        var command = new UpdateSaleCommand
        {
            Id = sale.Id,
            CustomerId = Guid.NewGuid(),
            BranchId = branch.Id,
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenSaleNotFound()
    {
        var branch = new Branch();
        branch.SetName("Branch 3");
        await _branchRepository.CreateAsync(branch);

        var customer = new Customer(Guid.NewGuid(), "Customer 3", "customer3@email", 3);
        branch.SetName("Customer 3");
        await _customerRepository.CreateAsync(customer);

        // Arrange
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            BranchId = branch.Id,
            CustomerId = customer.Id
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnResult_WhenSaleIsUpdatedSuccessfully()
    {
        // Arrange
        var branch = new Branch();
        branch.SetName("Branch 1");
        await _branchRepository.CreateAsync(branch);

        var customer = new Customer(Guid.NewGuid(), "Customer 1", "customer1@email", 1);
        branch.SetName("Customer 1");
        await _customerRepository.CreateAsync(customer);

        var sale = new Sale(branch.Id, customer.Id, new DateTime(2025, 4, 4));
        await _saleRepository.CreateAsync(sale);

        // Act & Assert

        var command = new UpdateSaleCommand { Id = sale.Id, BranchId = branch.Id, CustomerId = customer.Id, SaleItemCollection = new List<SaleItemCommand>() };
        
        var result = new UpdateSaleResult { 
            Id = sale.Id,
            BranchId = sale.BranchId,
            CustomerId= sale.CustomerId,
            Cancelled = sale.Cancelled,
            Date = sale.Date,
            Discount = sale.Discount,
            GrossTotal = sale.GrossTotal,
            Number = sale.Number,
            PercentageDiscount = sale.PercentageDiscount,
            Total = sale.Total,
            SaleItemCollection = sale.SaleItemCollection == null ? null : 
                                 sale.SaleItemCollection
                                     .Select( i => new SaleItemResult { ProductId = i.ProductId, Quantity =i.Quantity })
                                     .ToList()
            
        };

        _mapper.Map<UpdateSaleResult>(sale).Returns(result);

        var response = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(result, response);
        await _eventBus.Received(1).SendAsync(Arg.Any<SaleModifiedEvent>());
    }
}