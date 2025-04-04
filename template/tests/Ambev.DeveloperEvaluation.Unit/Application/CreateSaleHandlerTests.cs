using Xunit;
using AutoMapper;
using FluentValidation;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Infrastructure.Repositories;
using Ambev.DeveloperEvaluation.Unit;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = new SaleRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());
        _productRepository = new ProductRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());
        _branchRepository = new BranchRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());
        _customerRepository = new CustomerRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());
        _mapper = Substitute.For<IMapper>();
        _eventBus = Substitute.For<IEventBus>();

        _handler = new CreateSaleHandler(
            _saleRepository,
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
        // Arrange
        var command = new CreateSaleCommand
        {
            BranchId = Guid.Empty,
            CustomerId = Guid.Empty
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenBranchNotFound()
    {
        var customer = new Customer(Guid.NewGuid(), "Customer 1", "customer1@email", 1);

        await _customerRepository.CreateAsync(customer);

        // Arrange
        var command = new CreateSaleCommand
        {
            BranchId = Guid.NewGuid(),
            CustomerId = customer.Id
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

        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = branch.Id
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnResult_WhenSaleIsCreatedSuccessfully()
    {
        // Arrange

        var branch = new Branch(Guid.NewGuid(), "Branch 2", 1);
        await _branchRepository.CreateAsync(branch);

        var customer = new Customer(Guid.NewGuid(), "Customer 2", "customer2@email.com", 1);
        await _customerRepository.CreateAsync(customer);

        var command = new CreateSaleCommand
        {
            BranchId = branch.Id,
            CustomerId = customer.Id,
            Date = new DateTime(2025, 4, 4)
        };

        var sale = new Sale(command.BranchId, command.CustomerId, command.Date);
        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult { BranchId = command.BranchId, CustomerId = command.CustomerId, Id = sale.Id });
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        sale = (await _saleRepository.GetByIdAsync(result.Id));

        // Assert
        Assert.Equal(command.BranchId, sale.BranchId);
        Assert.Equal(command.CustomerId, sale.CustomerId);
        Assert.Equal(command.Date, sale.Date);

        await _eventBus.Received(1).SendAsync(Arg.Any<SaleCreatedEvent>());
    }
}