using Ambev.DeveloperEvaluation.Application.Branchs.ListBranchs;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.ListBranchs;

public class ListBranchsProfile : Profile
{
    public ListBranchsProfile()
    {
        CreateMap<ListBranchsRequest, ListBranchsCommand>();
        CreateMap<ListBranchResult, BranchResponse>();
    }
}
