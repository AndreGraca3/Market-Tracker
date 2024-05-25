namespace market_tracker_webapi.Application.Domain.Filters;

public class PaginatedResult<T>(IEnumerable<T> items, int totalItems, int skip, int take)
{
    public IEnumerable<T> Items { get; set; } = items;

    public int CurrentPage { get; set; } = skip / take + 1;

    public int ItemsPerPage { get; set; } = take;

    public int TotalItems { get; set; } = totalItems;

    public int TotalPages { get; set; } = (int)Math.Ceiling((double)totalItems / take);
    
    public PaginatedResult<TR> Select<TR>(Func<T, TR> selector)
    {
        return new PaginatedResult<TR>(
            Items.Select(selector),
            TotalItems,
            skip,
            take
        );
    }
}