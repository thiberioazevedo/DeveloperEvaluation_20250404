using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.ListBranchs;

public record ListBranchsCommand : IRequest<List<ListBranchResult>>
{
}
