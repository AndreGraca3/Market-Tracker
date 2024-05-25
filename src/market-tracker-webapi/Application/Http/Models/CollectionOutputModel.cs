namespace market_tracker_webapi.Application.Http.Models;

public record CollectionOutputModel<T>(IEnumerable<T> Items);

public static class CollectionModelMapper
{
    public static CollectionOutputModel<T> ToCollectionOutputModel<T>(this IEnumerable<T> items)
    {
        return new CollectionOutputModel<T>(items);
    }
}