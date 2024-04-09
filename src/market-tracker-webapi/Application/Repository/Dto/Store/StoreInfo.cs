using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto.City;

namespace market_tracker_webapi.Application.Repository.Dto.Store;

public record StoreInfo(
    int Id,
    string Name,
    string Address,
    CityInfo? City,
    CompanyInfo Company
)
{
    public static StoreInfo ToStoreInfo(
        Domain.Store store,
        Domain.City? city,
        Company company
    )
    {
        return new StoreInfo(store.Id, store.Name, store.Address, city is not null ? CityInfo.ToCity(city) : null,
            CompanyInfo.ToCompanyInfo(company));
    }
}