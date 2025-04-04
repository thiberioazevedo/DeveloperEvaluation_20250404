using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

public class ListCustomersProfile : Profile
{
    public ListCustomersProfile()
    {
        CreateMap<Domain.Entities.Customer, CustomerResult>();
    }
}
