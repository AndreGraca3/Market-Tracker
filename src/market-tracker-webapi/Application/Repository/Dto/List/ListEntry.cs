using market_tracker_webapi.Application.Repository.Dto.Store;

namespace market_tracker_webapi.Application.Repository.Dto.List;

public class ListEntry
{
    public required ProductItem ProductItem { get; set; }
    public required StorePrice StorePrice { get; set; }
    public int Quantity { get; set; }
}