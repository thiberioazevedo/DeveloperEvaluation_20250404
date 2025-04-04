using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

public class ListCustomersValidator : AbstractValidator<ListCustomersCommand>
{
    public ListCustomersValidator()
    {
    }
}
