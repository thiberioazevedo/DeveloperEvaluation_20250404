using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CDBs.ListCDBs;


public class ListPaginationRequestValidator<T>: AbstractValidator<T> where T : ListPaginationRequest
{
    public ListPaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than zero");
    }
}
