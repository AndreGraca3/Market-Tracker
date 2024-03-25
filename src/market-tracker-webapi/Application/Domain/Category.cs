namespace market_tracker_webapi.Application.Domain;

public record Category(int Id, string Name, int? ParentId, List<Category> Children);