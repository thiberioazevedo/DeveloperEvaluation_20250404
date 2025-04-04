namespace Ambev.DeveloperEvaluation.WebApi.Features;

public class ListPaginationRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SearchText { get; set; }
    public string? ColumnOrder { get; set; }
    public bool Asc { get; set; }
}
