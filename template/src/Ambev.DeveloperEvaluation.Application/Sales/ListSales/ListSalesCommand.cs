using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public record ListSalesCommand : IRequest<PaginatedList<SaleResult>>
{
    public string? SearchText { get; set; }
    public string? ColumnOrder { get; set; }
    public bool Asc { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
