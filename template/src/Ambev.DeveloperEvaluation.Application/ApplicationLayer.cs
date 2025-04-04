using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application;

/// <summary>
/// Represents the application layer of the system.
/// </summary>
/// <remarks>
/// This class serves as a reference for other layers when they need to access 
/// or reference the assembly of the application layer. The assembly can be 
/// retrieved using <c>typeof(ApplicationLayer).Assembly</c>, which allows 
/// other layers to programmatically reference the application layer's assembly.
/// </remarks>
public class ApplicationLayer {
    public static Domain.Repositories.PaginatedList<TDestination> MapPaginationList<TSource, TDestination>(Domain.Repositories.PaginatedList<TSource> response, IMapper mapper)
    {
        return new Domain.Repositories.PaginatedList<TDestination>
        {
            PageNumber = response.PageNumber,
            PageSize = response.PageSize,
            TotalCount = response.TotalCount,
            Collection = response.Collection
                                 .Select(i => mapper.Map<TDestination>(i))
                                 .ToList()
        };
    }
}