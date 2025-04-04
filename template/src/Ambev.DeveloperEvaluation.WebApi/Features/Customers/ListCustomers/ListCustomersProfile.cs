using Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.ListCustomers;

public class ListCustomersProfile : Profile
{
    public ListCustomersProfile()
    {
        CreateMap<ListCustomersRequest, ListCustomersCommand>();
        CreateMap<CustomerResult, CustomerResponse>();
    }
}
