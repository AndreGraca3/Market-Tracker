using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Feedback;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;
using market_tracker_webapi.Application.Utils;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class ProductFeedbackServiceTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IProductFeedbackRepository> _productFeedbackRepositoryMock;
    
    private readonly ProductFeedbackService _productFeedbackService;

    public ProductFeedbackServiceTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productFeedbackRepositoryMock = new Mock<IProductFeedbackRepository>();
        
        _productFeedbackService = new ProductFeedbackService(
            _productRepositoryMock.Object,
            _productFeedbackRepositoryMock.Object,
            new MockedTransactionManager()
        );
    }

    [Fact]
    public async Task GetReviewsByProductIdAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _productFeedbackService.GetReviewsByProductIdAsync("1", 0, 10)
        );
        
        // Assert
        Assert.IsType<ProductFetchingError.ProductByIdNotFound>(exception.ServiceError);
        
        _productRepositoryMock.Verify(x => x.GetProductByIdAsync(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task GetReviewsByProductIdAsync_ShouldReturnReviews()
    {
        var expectedListReviews = new List<ProductReview>
        {
            new(
                new ReviewId(1), 
                new ClientItem(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Client 1", "avatar1"),
                "1",
                5,
                "Good product",
                new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
                )
        };

        var expectedProduct = new Product(
            "1",
            "Product 1",
            "Description 1",
            10,
            ProductUnit.Units,
            1,
            10,
            new Brand(1, "Brand 1"),
            new Category(1, "Category 1")
        );
        
        // Arrange
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedProduct);
        
        _productFeedbackRepositoryMock.Setup(x => x.GetReviewsByProductIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new PaginatedResult<ProductReview>(expectedListReviews, 1, 0, 1));
        
        // Act
        var result = await _productFeedbackService.GetReviewsByProductIdAsync("1", 0, 10);
        
        // Assert
        result.Items.Should().BeEquivalentTo(expectedListReviews);
    }
    
    [Fact]
    public async Task UpsertProductPreferencesAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _productFeedbackService.UpsertProductPreferencesAsync(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "1",
                new Optional<bool>(),
                new Optional<ProductReviewInputModel>()
            )
        );
        
        // Assert
        Assert.IsType<ProductFetchingError.ProductByIdNotFound>(exception.ServiceError);
        
        _productRepositoryMock.Verify(x => x.GetProductByIdAsync(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task UpsertProductPreferencesAsync_ShouldReturnProductPreferences()
    {
        var expectedProductPreferences = new ProductPreferences(
            true,
            new ProductReview(
                new ReviewId(1),
                new ClientItem(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Client 1", "avatar1"),
                "1",
                5,
                "Good product",
                new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            )
        );
        
        
        // Arrange
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Product(
                "1",
                "Product 1",
                "Description 1",
                10,
                ProductUnit.Units,
                1,
                10,
                new Brand(1, "Brand 1"),
                new Category(1, "Category 1")
            ));
        
        _productFeedbackRepositoryMock.Setup(x => x.GetProductPreferencesAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(expectedProductPreferences);
        
        _productFeedbackRepositoryMock.Setup(x => x.UpdateReviewAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new ProductReview(
                new ReviewId(1), 
                new ClientItem(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Client 1", "avatar1"),
                "1",
                5,
                "Good product",
                new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            ));
        
        // Act
        var result = await _productFeedbackService.UpsertProductPreferencesAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            new Optional<bool>(),
            new Optional<ProductReviewInputModel>(
                new ProductReviewInputModel(5, "Good product")
            )
        );
        
        // Assert
        result.Should().BeEquivalentTo(expectedProductPreferences);
    }
    
    [Fact]
    public async Task UpsertProductPreferencesAsync_ShouldReturnProductPreferencesWithNewReview()
    {
        var expectedProductPreferences = new ProductPreferences(
            true,
            new ProductReview(
                new ReviewId(1),
                new ClientItem(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Client 1", "avatar1"),
                "1",
                5,
                "Good product",
                new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            )
        );
        
        
        // Arrange
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Product(
                "1",
                "Product 1",
                "Description 1",
                10,
                ProductUnit.Units,
                1,
                10,
                new Brand(1, "Brand 1"),
                new Category(1, "Category 1")
            ));
        
        _productFeedbackRepositoryMock.Setup(x => x.GetProductPreferencesAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(new ProductPreferences(
                true,
                null
            ));
        
        _productFeedbackRepositoryMock.Setup(x => x.AddReviewAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new ReviewId(1));
        
        _productFeedbackRepositoryMock.Setup(x => x.GetReviewByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ProductReview(
                new ReviewId(1), 
                new ClientItem(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Client 1", "avatar1"),
                "1",
                5,
                "Good product",
                new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            ));
        
        // Act
        var result = await _productFeedbackService.UpsertProductPreferencesAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            new Optional<bool>(),
            new Optional<ProductReviewInputModel>(
                new ProductReviewInputModel(5, "Good product")
            )
        );
        
        // Assert
        result.Should().BeEquivalentTo(expectedProductPreferences);
    }
    
    [Fact]
    public async Task GetFavouritesAsync_ShouldReturnFavourites()
    {
        var expectedListProductItems = new List<ProductItem>
        {
            new ProductItem(new ProductId("1"), "Product 1", "Description 1", "Brand 1")
        };
        
        // Arrange
        _productFeedbackRepositoryMock.Setup(x => x.GetFavouriteProductsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedListProductItems);
        
        // Act
        var result = await _productFeedbackService.GetFavouritesAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        result.Should().BeEquivalentTo(expectedListProductItems);
    }
    
    [Fact]
    public async Task GetFavouritesAsync_ShouldReturnEmptyList()
    {
        // Arrange
        _productFeedbackRepositoryMock.Setup(x => x.GetFavouriteProductsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<ProductItem>());
        
        // Act
        var result = await _productFeedbackService.GetFavouritesAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetProductPreferencesAsync_ShouldReturnProductPreferences()
    {
        var expectedProduct = new Product(
            "1",
            "Product 1",
            "Description 1",
            10,
            ProductUnit.Units,
            1,
            10,
            new Brand(1, "Brand 1"),
            new Category(1, "Category 1")
        );
        
        var expectedProductPreferences = new ProductPreferences(
            true,
            new ProductReview(
                new ReviewId(1),
                new ClientItem(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Client 1", "avatar1"),
                "1",
                5,
                "Good product",
                new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            )
        );
        
        // Arrange
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedProduct);
        
        _productFeedbackRepositoryMock.Setup(x => x.GetProductPreferencesAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(expectedProductPreferences);
        
        // Act
        var result = await _productFeedbackService.GetProductPreferencesAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1"
        );
        
        // Assert
        result.Should().BeEquivalentTo(expectedProductPreferences);
    }
    
    [Fact]
    public async Task GetProductPreferencesAsync_ShouldReturnProductPreferencesWithNullReview()
    {
        var expectedProduct = new Product(
            "1",
            "Product 1",
            "Description 1",
            10,
            ProductUnit.Units,
            1,
            10,
            new Brand(1, "Brand 1"),
            new Category(1, "Category 1")
        );
        
        var expectedProductPreferences = new ProductPreferences(
            true,
            null
        );
        
        // Arrange
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedProduct);
        
        _productFeedbackRepositoryMock.Setup(x => x.GetProductPreferencesAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(expectedProductPreferences);
        
        // Act
        var result = await _productFeedbackService.GetProductPreferencesAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1"
        );
        
        // Assert
        result.Should().BeEquivalentTo(expectedProductPreferences);
    }
    
    [Fact]
    public async Task GetProductPreferencesAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _productFeedbackService.GetProductPreferencesAsync(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "1"
            )
        );
        
        // Assert
        Assert.IsType<ProductFetchingError.ProductByIdNotFound>(exception.ServiceError);
        
        _productRepositoryMock.Verify(x => x.GetProductByIdAsync(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task GetProductStatsByIdAsync_ShouldReturnProductStats()
    {
        var expectedProductStats = new ProductStats(
            "1",
            new ProductStatsCounts(1,10),
            10
        );
        
        // Arrange
        _productFeedbackRepositoryMock.Setup(x => x.GetProductStatsByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedProductStats);
        
        // Act
        var result = await _productFeedbackService.GetProductStatsByIdAsync("1");
        
        // Assert
        result.Should().BeEquivalentTo(expectedProductStats);
    }
    
    [Fact]
    public async Task GetProductStatsByIdAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productFeedbackRepositoryMock.Setup(x => x.GetProductStatsByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ProductStats)null!);
        
        // Act
        var exception = await Assert.ThrowsAsync<MarketTrackerServiceException>(() =>
            _productFeedbackService.GetProductStatsByIdAsync("1")
        );
        
        // Assert
        Assert.IsType<ProductFetchingError.ProductByIdNotFound>(exception.ServiceError);
        
        _productFeedbackRepositoryMock.Verify(x => x.GetProductStatsByIdAsync(It.IsAny<string>()), Times.Once);
    }
}