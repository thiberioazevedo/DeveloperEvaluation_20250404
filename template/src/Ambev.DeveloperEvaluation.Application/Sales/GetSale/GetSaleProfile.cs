using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<GetSaleCommand, Sale>();
        CreateMap<Sale, GetSaleResult>();
        CreateMap<SaleItem, SaleItemResult>();
        CreateMap<Branch, BranchResult>();
        CreateMap<Customer, CustomerResult>();
    }
}
