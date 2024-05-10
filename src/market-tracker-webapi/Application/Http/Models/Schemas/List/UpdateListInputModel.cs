namespace market_tracker_webapi.Application.Http.Models.Schemas.List;

public class UpdateListInputModel
{
    public string? ListName { get; set; }
    public bool? IsArchived { get; set; }
}