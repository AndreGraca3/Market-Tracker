using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;

namespace market_tracker_webapi_test.Application.Service;

public static class MockedData
{
    public static readonly List<Client> DummyClients =
    [
        new Client(
            Guid.NewGuid(),
            "client1master",
            "Client 1",
            "client1_email",
            DateTime.UtcNow - TimeSpan.FromDays(1),
            "avatar1"
        ),
        new Client(
            Guid.NewGuid(),
            "client2master",
            "Client 2",
            "client2_email",
            DateTime.UtcNow - TimeSpan.FromDays(1),
            "avatar2"
        )
    ];
    
    public static readonly List<Company> DummyCompanies =
    [
        new Company(
            1,
            "Company 1",
            "company1_logo",
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        ),

        new Company(
            2,
            "Company 2",
            "company2_logo",
            new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Unspecified)
        )
    ];

    public static readonly List<Store> DummyStores =
    [
        new Store(1, "Store 1", "Address 1", new City(1, "city1"),
            new Company(1, "company1", "company1", DateTime.UtcNow), Guid.NewGuid()),


        new Store(2, "Store 2", "Address 2", new City(2, "city2"),
            new Company(2, "company2", "company2", DateTime.UtcNow), Guid.NewGuid())
    ];

    public static readonly List<Product> DummyProducts =
    [
        new Product("1", "Filipinos", "dummy_image_url", 1, ProductUnit.Units, 0, 0, new Brand(1, "no_brand"),
            new Category(1, "no_category")),
        new Product("2", "Atum", "dummy_image_url", 1, ProductUnit.Units, 0, 0, new Brand(1, "no_brand"),
            new Category(1, "no_category")),
        new Product("3", "Leite com atum", "dummy_image_url", 1, ProductUnit.Centiliters, 0, 0,
            new Brand(1, "no_brand"),
            new Category(1, "no_category")),
        new Product("4", "Filetes no espeto", "dummy_image_url", 1, ProductUnit.Units, 0, 0, new Brand(1, "no_brand"),
            new Category(1, "no_category"))
    ];

    public static readonly ProductsFacetsCounters DummyFacets = new(
        new List<FacetCounter>(),
        new List<FacetCounter>(),
        new List<FacetCounter>()
    );

    public static readonly List<ShoppingList> DummyLists =
    [
        new ShoppingList(
            "1",
            "List 1 with members",
            null,
            DateTime.UtcNow - TimeSpan.FromDays(1),
            DummyClients[0].Id.Value,
            [DummyClients[1].Id.Value]
        ),
        new ShoppingList(
            "2",
            "List 2 without members",
            DateTime.UtcNow - TimeSpan.FromDays(1),
            DateTime.UtcNow - TimeSpan.FromDays(3),
            DummyClients[0].Id.Value,
            []
        ),
        new ShoppingList(
            "3",
            "List 3 from user 2 without members",
            DateTime.UtcNow - TimeSpan.FromDays(1),
            DateTime.UtcNow - TimeSpan.FromDays(2),
            DummyClients[1].Id.Value,
            []
        )
    ];

    public static readonly List<ListEntry> DummyListEntries =
    [
        new ListEntry("1", DummyProducts[0], DummyStores[0].ToStoreItem(), 1),
        new ListEntry("2", DummyProducts[1], DummyStores[0].ToStoreItem(), 1),
        new ListEntry("3", DummyProducts[2], DummyStores[1].ToStoreItem(), 1)
    ];
}