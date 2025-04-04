using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Branchs.ListBranchs;

public class ListBranchsProfile : Profile
{
    public ListBranchsProfile()
    {
        CreateMap<Domain.Entities.Branch, ListBranchResult>();
    }
}
