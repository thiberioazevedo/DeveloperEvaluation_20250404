using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public class PaginatedRequestBaseList()
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class PaginatedList<T>: PaginatedRequestBaseList
{
    public int TotalPages
    {
        get
        {
            var totalPages = TotalCount / PageSize;

            if (TotalCount % PageSize > 0)
                totalPages++;

            return totalPages;
        }
    }
    public int TotalCount { get; set; }
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
    public ICollection<T>? Collection { get; set; }
    public static async Task<PaginatedList<T>> CreateInstanceAsync(int pageNumber, int pageSize, IQueryable<T> queryable)
    {
        var paginatedList = new PaginatedList<T>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = await queryable.CountAsync(),
        };
        
        var queryablePaginated = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        paginatedList.Collection = await queryablePaginated.ToListAsync();
        
        return paginatedList;
    }
}