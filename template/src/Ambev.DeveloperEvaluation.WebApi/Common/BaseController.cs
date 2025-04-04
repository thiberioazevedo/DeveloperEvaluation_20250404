using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    private readonly IMapper _mapper;

    public BaseController(IMapper mapper)
    {
        _mapper = mapper;
    }

    protected int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

    protected IActionResult Ok<T>(T data, string message = "Success") =>
        base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult Ok(string message = "Resource not found") =>
        base.Ok(new ApiResponse { Message = message, Success = true });

    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ApiResponse { Message = message, Success = false });

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ApiResponse { Message = message, Success = false });

    protected IActionResult BadRequest(List<ValidationFailure> errors) =>
        base.BadRequest(new ApiResponse
        {
            Success = false,
            Message = "Validation Failed",
            Errors = errors.Select(o => new ValidationErrorDetail { Detail = o.ErrorMessage, Error = o.ErrorCode })
        });

    protected IActionResult OkPaginatedList<TSource, TDestination>(Domain.Repositories.PaginatedList<TSource> response)
    {
        return Ok(Application.ApplicationLayer.MapPaginationList<TSource, TDestination>(response, _mapper));
    }
}
