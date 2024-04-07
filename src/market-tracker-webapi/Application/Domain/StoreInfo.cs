namespace market_tracker_webapi.Application.Domain;

public record StoreInfo(
    int Id,
    string Name,
    string Address,
    Domain.City? City,
    Domain.Company Company
)
{
    public static StoreInfo ToStoreInfo(
        Domain.Store store,
        Domain.City? city,
        Domain.Company company
    )
    {
        return new StoreInfo(store.Id, store.Name, store.Address, city, company);
    }
}
