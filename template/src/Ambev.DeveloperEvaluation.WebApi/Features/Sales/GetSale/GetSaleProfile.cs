using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<Guid, GetSaleCommand>().ConstructUsing(id => new GetSaleCommand(id));

        CreateMap<BranchResult, BranchResponse>();
        CreateMap<CustomerResult, CustomerResponse>();
        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<SaleItemResult, SaleItemResponse>();

    }
}
