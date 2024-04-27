namespace market_tracker_webapi.Application.Http.Models.List;

public class ListCreationInputModel
{
    public Guid ClientId { get; set; }
    public required string ListName { get; set; }
}