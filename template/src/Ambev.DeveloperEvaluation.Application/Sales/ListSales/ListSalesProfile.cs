using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesProfile : Profile
{
    public ListSalesProfile()
    {
        CreateMap<Sale, SaleResult>();
        CreateMap<Customer, CustomerResult>();
        CreateMap<Branch, BranchResult>();
    }
}
