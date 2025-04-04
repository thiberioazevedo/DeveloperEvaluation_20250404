using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker = new Faker<CreateSaleCommand>()
        //.RuleFor(u => u.Salename, f => f.Internet.SaleName())
        //.RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
        //.RuleFor(u => u.Email, f => f.Internet.Email())
        //.RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        //.RuleFor(u => u.Status, f => f.PickRandom(SaleStatus.Active, SaleStatus.Suspended))
        //.RuleFor(u => u.Role, f => f.PickRandom(SaleRole.Customer, SaleRole.Admin))
        ;

    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleHandlerFaker.Generate();
    }
}
