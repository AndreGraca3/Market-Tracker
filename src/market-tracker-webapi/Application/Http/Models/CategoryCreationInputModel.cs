namespace market_tracker_webapi.Application.Http.Models;

public record CategoryCreationInputModel(string Name, int? parentId = null);