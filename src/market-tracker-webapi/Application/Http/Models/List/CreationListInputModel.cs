namespace market_tracker_webapi.Application.Http.Models.List;

public class CreationListInputModel
{
    public Guid ClientId { get; set; }
    public required string ListName { get; set; }
}