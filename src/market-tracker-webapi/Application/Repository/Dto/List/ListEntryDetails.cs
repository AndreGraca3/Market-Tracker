using market_tracker_webapi.Application.Repository.Dto.Store;

namespace market_tracker_webapi.Application.Repository.Dto.List;

public class ListEntryDetails
{
    public required ProductItem ProductItem { get; set; }
    public StorePrice? StorePrice { get; set; }
    public required int Quantity { get; set; }

    public bool IsAvailable { get; set; }
}