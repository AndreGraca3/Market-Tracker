namespace market_tracker_webapi.Application.Http.Models;

public record CollectionOutputModel<T>(IEnumerable<T> Items);