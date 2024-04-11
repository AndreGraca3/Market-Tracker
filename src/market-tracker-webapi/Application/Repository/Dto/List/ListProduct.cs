namespace market_tracker_webapi.Application.Repository.Dto.List;

public class ListProduct
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime? ArchivedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public required IEnumerable<ListEntryDetails> Products { get; set; }
    public int TotalPrice { get; set; }
    public int TotalProducts { get; set; }
}