using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Branchs.ListBranchs;

public class ListBranchsHandler : IRequestHandler<ListBranchsCommand, List<ListBranchResult>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public ListBranchsHandler(
        IBranchRepository branchRepository,
        IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<List<ListBranchResult>> Handle(ListBranchsCommand request, CancellationToken cancellationToken)
    {
        var validator = new ListBranchsValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var branchList = await _branchRepository.GetAllAsync();
        
        return _mapper.Map<List<ListBranchResult>>(branchList);
    }
}
