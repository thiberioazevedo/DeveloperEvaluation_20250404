using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomers;

public class ListCustomersRequestValidator : AbstractValidator<ListCustomersRequest>
{
    public ListCustomersRequestValidator()
    {
    }
}
