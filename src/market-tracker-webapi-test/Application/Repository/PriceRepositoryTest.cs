using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi_test.Application.Repository;

/*public class PriceRepositoryTest
{
    private static readonly List<ProductEntity> _dummyProducts =
    [
        new ProductEntity
        {
            Id = "1",
            Name = "Filipinos",
            ImageUrl = "dummy_image_url",
            Quantity = 1,
            Unit = "unidades",
            BrandId = 1,
            CategoryId = 12
        },
        new ProductEntity
        {
            Id = "2",
            Name = "Danoninho",
            ImageUrl = "dummy_image_url",
            Quantity = 1,
            Unit = "unidades",
            BrandId = 2,
            CategoryId = 12
        }
    ];
    
    private static readonly List<CityEntity> _dummyCities =
    [
        new CityEntity
        {
            Id = 1,
            Name = "Cacem"
        },
        new CityEntity
        {
            Id = 2,
            Name = "Lisboa"
        }
    ];
    
    private static readonly List<CompanyEntity> _dummyCompanies =
    [
        new CompanyEntity
        {
            Id = 1,
            Name = "Continente"
        },
        new CompanyEntity
        {
            Id = 2,
            Name = "Auchan"
        }
    ];
    
    private static readonly List<StoreEntity> _dummyStores =
    [
        new StoreEntity
        {
            Id = 1,
            Name = "Continente Online",
            CompanyId = 1,
            CityId = null,
            Address = "www.continente.pt"
        },
        new StoreEntity
        {
            Id = 2,
            Name = "Auchan do cacem",
            CompanyId = 2,
            CityId = 1,
            Address = "Cacem de baixo"
        }
    ];
    
    private static readonly List<PriceEntryEntity> _dummyPrices =
    [
        new PriceEntryEntity
        {
            ProductId = "1",
            StoreId = 1,
            Price = 135,
            CreatedAt = DateTime.Now - TimeSpan.FromDays(1)   
        },
        new PriceEntryEntity
        {
            ProductId = "1",
            StoreId = 1,
            Price = 1,
            CreatedAt = DateTime.Now
        },
        new PriceEntryEntity
        {
            ProductId = "2",
            StoreId = 2,
            Price = 53,
            CreatedAt = DateTime.Now - TimeSpan.FromDays(1)
        },
        new PriceEntryEntity
        {
            ProductId = "2",
            StoreId = 2,
            Price = 2,
            CreatedAt = DateTime.Now
        }
    ];
    
    private static readonly List<ProductAvailabilityEntity> _dummyStoreAvailabilities =
    [
        new ProductAvailabilityEntity
        {
            ProductId = "1",
            StoreId = 1,
            IsAvailable = true
        },
        new ProductAvailabilityEntity
        {
            ProductId = "1",
            StoreId = 2,
            IsAvailable = false
        },
        new ProductAvailabilityEntity
        {
            ProductId = "2",
            StoreId = 1,
            IsAvailable = true
        },
        new ProductAvailabilityEntity
        {
            ProductId = "2",
            StoreId = 2,
            IsAvailable = true
        }
    ];

    [Fact]
    public async Task GetAvailableProductsOffersAsync_ReturnsObjectAsync()
    {
        // Arrange
        var offers = _dummyProducts.Select(p =>
        {
            var store = _dummyStores.First();
            var price = _dummyPrices.First();
            var availability = _dummyStoreAvailabilities.First();
            return new ProductOffer(
                ProductInfo.ToProductInfo(ProductDetails.ToProductDetails(p, ))),
                new StoreOffer(StoreInfo.ToStoreInfo(store.ToStore()), price),

        var paginatedOffers = new PaginatedResult<ProductOffer>();
        var expectedProductOffers = new PaginatedProductOffers();

        var context = DbHelper.CreateDatabase(_dummyProducts);
        var priceRepo = new PriceRepository(context);

        // Act
        var actualProductOffers = await priceRepo.GetBestAvailableProductsOffersAsync(0, 10);

        // Assert
        actualProductOffers.Should().BeEquivalentTo(expectedProductOffers);
    }
}*/