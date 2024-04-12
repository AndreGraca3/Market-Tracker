namespace market_tracker_webapi.Application.Http.Models.List;

public class UpdateListInputModel
{
    public string? ListName { get; set; }
    public DateTime? ArchivedAt { get; set; }
}