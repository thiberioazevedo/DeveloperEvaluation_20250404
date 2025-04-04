using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.ListBranchs;
using Ambev.DeveloperEvaluation.Application.Branchs.ListBranchs;
using Microsoft.AspNetCore.Authorization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs;

[ApiController]
[Route("api/[controller]")]
public class BranchsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BranchsController(IMediator mediator, IMapper mapper) : base(mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<BranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> ListBranchs(CancellationToken cancellationToken)
    {
        var request = new ListBranchsRequest {  };
        var validator = new ListBranchsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<ListBranchsCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result.Select(i => _mapper.Map<BranchResponse>(i)).ToList());
    }
}
