using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Market.Alert;
using market_tracker_webapi.Application.Repository.Market.Inventory.Brand;
using market_tracker_webapi.Application.Repository.Market.Inventory.Category;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.External;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;
using market_tracker_webapi.Application.Service.Results;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class ProductServiceTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IBrandRepository> _brandRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IPriceRepository> _priceRepositoryMock;
    private readonly Mock<IStoreRepository> _storeRepositoryMock;
    private readonly Mock<IClientDeviceRepository> _clientDeviceRepositoryMock;
    private readonly Mock<IPriceAlertRepository> _priceAlertRepositoryMock;

    private readonly Mock<INotificationService> _notificationServiceMock;

    private readonly ProductService _productService;

    public ProductServiceTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _priceRepositoryMock = new Mock<IPriceRepository>();
        _storeRepositoryMock = new Mock<IStoreRepository>();
        _clientDeviceRepositoryMock = new Mock<IClientDeviceRepository>();
        _priceAlertRepositoryMock = new Mock<IPriceAlertRepository>();

        _notificationServiceMock = new Mock<INotificationService>();

        _productService = new ProductService(
            _productRepositoryMock.Object,
            _brandRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _priceRepositoryMock.Object,
            _storeRepositoryMock.Object,
            _clientDeviceRepositoryMock.Object,
            _priceAlertRepositoryMock.Object,
            _notificationServiceMock.Object,
            new MockedTransactionManager()
        );
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnProducts()
    {
        // Arrange
        _productRepositoryMock.Setup(repo => repo.GetAvailableProductsAsync(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<ProductsSortOption?>(),
            It.IsAny<string?>(),
            It.IsAny<IList<int>?>(),
            It.IsAny<IList<int>?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<IList<int>?>(),
            It.IsAny<IList<int>?>(),
            It.IsAny<IList<int>?>()
        )).ReturnsAsync(new PaginatedFacetedProducts(
            new PaginatedResult<Product>(
                MockedData.DummyProducts,
                MockedData.DummyProducts.Count,
                0,
                MockedData.DummyProducts.Count
            ),
            new ProductsFacetsCounters(new List<FacetCounter>(), new List<FacetCounter>(), new List<FacetCounter>())
        ));

        var offer = new StoreOffer(
            MockedData.DummyStores[0],
            new Price(10, null, DateTime.Now),
            new StoreAvailability(MockedData.DummyStores[0].Id.Value,
                It.IsAny<string>(),
                true,
                DateTime.Now
            ));

        _priceRepositoryMock.Setup(repo => repo.GetCheapestStoreOfferAvailableByProductIdAsync(
            It.IsAny<string>(),
            It.IsAny<IList<int>?>(),
            It.IsAny<IList<int>?>(),
            It.IsAny<IList<int>?>()
        )).ReturnsAsync(offer);

        // Act
        var paginatedProductOffers =
            await _productService.GetBestAvailableProductsOffersAsync(
                0,
                MockedData.DummyProducts.Count,
                It.IsAny<int>(),
                It.IsAny<ProductsSortOption?>(),
                It.IsAny<string?>(),
                It.IsAny<IList<int>?>(),
                It.IsAny<IList<int>?>(),
                It.IsAny<IList<int>?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()
            );

        // Assert
        foreach (var productOffer in paginatedProductOffers.Items)
        {
            productOffer
                .Should()
                .BeEquivalentTo(productOffer with { StoreOffer = offer });
        }

        paginatedProductOffers.Facets
            .Should()
            .BeEquivalentTo(MockedData.DummyFacets);
    }

    [Fact]
    public async Task GetProductAsync_ShouldReturnProduct()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        // Act
        var productResult = await _productService.GetProductByIdAsync(It.IsAny<string>());

        // Assert
        productResult
            .Should()
            .BeEquivalentTo(MockedData.DummyProducts[0]);
    }

    [Fact]
    public async Task GetProductAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _productService.GetProductByIdAsync(It.IsAny<string>()));

        // Assert
        ex
            .ServiceError
            .Should()
            .BeEquivalentTo(new ProductFetchingError.ProductByIdNotFound(It.IsAny<string>()));
    }

    [Fact]
    public async Task AddExistingProductAsync_ShouldReturnProductCreationResultWithCreatedStatus()
    {
        // Arrange
        var newProduct = new Product(
            "newId",
            "newName",
            "newImageUrl",
            1,
            ProductUnit.Units,
            0,
            0,
            new Brand(69, "newBrand"),
            new Category(69, "no_category")
        );

        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product?)null);

        _brandRepositoryMock
            .Setup(repo => repo.GetBrandByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0].Brand);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyProducts[0].Category);

        _storeRepositoryMock.Setup(repo => repo.GetStoreByOperatorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _productRepositoryMock
            .Setup(repo => repo.AddProductAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            ))
            .ReturnsAsync((ProductId)null!);

        // Act
        var productResult = await _productService.AddProductAsync(
            It.IsAny<Guid>(),
            newProduct.Id.Value,
            newProduct.Name,
            newProduct.ImageUrl,
            newProduct.Quantity,
            newProduct.Unit,
            newProduct.Brand.Name,
            newProduct.Category.Id.Value,
            10,
            It.IsAny<int?>()
        );

        // Assert
        productResult
            .Should()
            .BeEquivalentTo(new ProductCreationResult
            {
                Id = newProduct.Id.Value,
                IsNew = true,
                PriceChanged = true
            });
    }

    [Fact]
    public async Task AddExistingProductAsync_ShouldReturnProductCreationResultWithUpdatedStatus()
    {
        // Arrange
        var newProduct = new Product(
            "newId",
            "newName",
            "newImageUrl",
            1,
            ProductUnit.Units,
            0,
            0,
            new Brand(69, "newBrand"),
            new Category(69, "no_category")
        );

        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _brandRepositoryMock
            .Setup(repo => repo.GetBrandByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0].Brand);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyProducts[0].Category);

        _storeRepositoryMock.Setup(repo => repo.GetStoreByOperatorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _productRepositoryMock
            .Setup(repo => repo.AddProductAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            ))
            .ReturnsAsync((ProductId)null!);

        // Act
        var productResult = await _productService.AddProductAsync(
            It.IsAny<Guid>(),
            newProduct.Id.Value,
            newProduct.Name,
            newProduct.ImageUrl,
            newProduct.Quantity,
            newProduct.Unit,
            newProduct.Brand.Name,
            newProduct.Category.Id.Value,
            10,
            It.IsAny<int?>()
        );

        // Assert
        productResult
            .Should()
            .BeEquivalentTo(new ProductCreationResult
            {
                Id = newProduct.Id.Value,
                IsNew = false,
                PriceChanged = true
            });
    }

    [Fact]
    public async Task AddProductAsync_ShouldReturnCategoryByIdNotFound()
    {
        // Arrange
        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Category?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _productService.AddProductAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<ProductUnit>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int?>()
            ));

        // Assert
        ex
            .ServiceError
            .Should()
            .BeEquivalentTo(new CategoryFetchingError.CategoryByIdNotFound(It.IsAny<int>()));
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldReturnProduct()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _brandRepositoryMock
            .Setup(repo => repo.GetBrandByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Brand?)null);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyProducts[0].Category);

        _productRepositoryMock
            .Setup(repo => repo.UpdateProductAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            ))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        // Act
        var productResult = await _productService.UpdateProductAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<ProductUnit>(),
            It.IsAny<string>(),
            It.IsAny<int>()
        );

        // Assert
        productResult
            .Should()
            .BeEquivalentTo(MockedData.DummyProducts[0]);
    }
    
    [Fact]
    public async Task UpdateProductAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _productService.UpdateProductAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<ProductUnit>(),
                It.IsAny<string>(),
                It.IsAny<int>()
            ));

        // Assert
        ex
            .ServiceError
            .Should()
            .BeEquivalentTo(new ProductFetchingError.ProductByIdNotFound(It.IsAny<string>()));
    }
    
    [Fact]
    public async Task UpdateProductAsync_ShouldReturnCategoryByIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Category?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _productService.UpdateProductAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<ProductUnit>(),
                It.IsAny<string>(),
                It.IsAny<int>()
            ));

        // Assert
        ex
            .ServiceError
            .Should()
            .BeEquivalentTo(new CategoryFetchingError.CategoryByIdNotFound(It.IsAny<int>()));
    }

    [Fact]
    public async Task RemoveProductAsync_ShouldReturnProductId()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _productRepositoryMock
            .Setup(repo => repo.RemoveProductAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        // Act
        var productResult = await _productService.RemoveProductAsync(It.IsAny<string>());

        // Assert
        productResult
            .Should()
            .BeEquivalentTo(MockedData.DummyProducts[0].Id);
    }

    [Fact]
    public async Task RemoveProductAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _productService.RemoveProductAsync(It.IsAny<string>()));

        // Assert
        ex
            .ServiceError
            .Should()
            .BeEquivalentTo(new ProductFetchingError.ProductByIdNotFound(It.IsAny<string>()));
    }

    [Fact]
    public async Task SetProductAvailabilityStatusAsync_ShouldReturnProductId()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _storeRepositoryMock
            .Setup(repo => repo.GetStoreByOperatorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockedData.DummyStores[0]);
        
        // Act
        var productResult =
            await _productService.SetProductAvailabilityAsync(
                It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>());

        // Assert
        productResult
            .Should()
            .BeEquivalentTo(MockedData.DummyProducts[0].Id);
    }
    
    [Fact]
    public async Task SetProductAvailabilityStatusAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product?)null);
        
        _storeRepositoryMock
            .Setup(repo => repo.GetStoreByOperatorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _productService.SetProductAvailabilityAsync(
                It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()));

        // Assert
        ex
            .ServiceError
            .Should()
            .BeEquivalentTo(new ProductFetchingError.ProductByIdNotFound(It.IsAny<string>()));
    }
    
    [Fact]
    public async Task SetProductAvailabilityStatusAsync_ShouldReturnStoreByOperatorIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);
        
        _storeRepositoryMock
            .Setup(repo => repo.GetStoreByOperatorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Store?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _productService.SetProductAvailabilityAsync(
                It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()));

        // Assert
        ex
            .ServiceError
            .Should()
            .BeEquivalentTo(new StoreFetchingError.StoreByOperatorIdNotFound(It.IsAny<Guid>()));
    }
}