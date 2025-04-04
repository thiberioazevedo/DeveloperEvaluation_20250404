using Xunit;
using AutoMapper;
using FluentValidation;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Unit;
using Ambev.DeveloperEvaluation.Infrastructure.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;

public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEventBus _eventBus;
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTests()
    {
        _userRepository = new UserRepository(BaseTestHelpers.GetDefaultContextInstanceInMemoryDatabase());
        _mapper = Substitute.For<IMapper>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _eventBus = Substitute.For<IEventBus>();

        _handler = new CreateUserHandler(
            _userRepository,
            _mapper,
            _passwordHasher,
            _eventBus
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenValidationFails()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "",
            Password = ""
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserAlreadyExists()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            Password = "!@#$%QWERTasdfg1ord",
            Username = "test",
            Phone = "99999999999",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };

        await _userRepository.CreateAsync(new User(command.Username, command.Email, command.Phone, command.Password, command.Role, command.Status), CancellationToken.None);

        _passwordHasher.HashPassword(command.Password).Returns("hashedPassword");
        _mapper.Map<User>(command).Returns(new User());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnResult_WhenUserIsCreatedSuccessfully()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test2@example.com",
            Password = "!@#$%QWERTasdfg1ord",
            Username = "test2",
            Phone = "99999999999",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };

        var user = new User(command.Username, command.Email, command.Phone, command.Password, command.Role, command.Status);
        var result = new CreateUserResult();

        _mapper.Map<User>(command).Returns(user);
        _passwordHasher.HashPassword(command.Password).Returns("hashedPassword");

        _mapper.Map<CreateUserResult>(user).Returns(result);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(result, response);
        await _eventBus.Received(1).SendAsync(Arg.Any<UserCreatedEvent>());
    }
}