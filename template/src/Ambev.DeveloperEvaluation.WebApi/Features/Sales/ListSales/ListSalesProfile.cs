using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

public class ListSalesProfile : Profile
{
    public ListSalesProfile()
    {
        CreateMap<ListSalesRequest, ListSalesCommand>();
        CreateMap<SaleResult, SaleResponse>();
        CreateMap<CustomerResult, CustomerResponse>();
        CreateMap<BranchResult, BranchResponse>();
    }
}
