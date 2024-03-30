using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Operations.Brand;
using market_tracker_webapi.Application.Repository.Operations.Category;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Operations.Product;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;
using Moq;

namespace market_tracker_webapi_test.Application.Services;

public class ProductServiceTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IBrandRepository> _brandRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<ITransactionManager> _transactionManagerMock;

    private readonly ProductService _productService;

    public ProductServiceTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _transactionManagerMock = new Mock<ITransactionManager>();

        _productService = new ProductService(
            _productRepositoryMock.Object,
            _brandRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _transactionManagerMock.Object
        );
    }

    private readonly List<Product> _dummyProducts =
    [
        new Product(1, "Filipinos", "dummy_image_url", 1, "unidades", 0, 0, 1, 12),
        new Product(2, "Maça", "dummy_image_url", 1, "unidades", 0, 0, 2, 9),
        new Product(3, "Gomas", "dummy_image_url", 1, "unidades", 0, 0, 3, 5)
    ];

    private readonly List<Brand> _dummyBrands =
    [
        new Brand(1, "dummy_brand"),
        new Brand(2, "dummy_brand"),
        new Brand(3, "dummy_brand")
    ];

    private readonly List<Category> _dummyCategories =
    [
        new Category(12, "dummy_category"),
        new Category(9, "dummy_category"),
        new Category(5, "dummy_category")
    ];

    /*
    [Fact]
    public async Task GetCitiesAsync_ShouldReturnCities() {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new Product(1, "Filipinos", "dummy_image_url", 1, "unidades", 0, 0, 1, 12),
            new Product(2, "Maça", "dummy_image_url", 1, "unidades", 0, 0, 2, 9),
            new Product(3, "Gomas", "dummy_image_url", 1, "unidades", 0, 0, 3, 5)
        };

        _productRepositoryMock
            .Setup(repo => repo.GetProductsAsync())
            .ReturnsAsync(expectedProducts);

        // Act
        var actualProducts = await _productService.GetProductsAsync();

        // Assert
        actualProducts.Should().BeEquivalentTo(expectedProducts);
    }
    */

    [Fact]
    public async Task GetProductAsync_ShouldReturnProduct()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(_dummyProducts[0].Id))
            .ReturnsAsync(_dummyProducts[0]);

        _brandRepositoryMock
            .Setup(repo => repo.GetBrandByIdAsync(_dummyProducts[0].BrandId))
            .ReturnsAsync(_dummyBrands[0]);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(_dummyProducts[0].CategoryId))
            .ReturnsAsync(_dummyCategories[0]);

        // Act
        var productResult = await _productService.GetProductAsync(_dummyProducts[0].Id);

        // Assert
        productResult
            .Value.Should()
            .BeEquivalentTo(
                ProductOutputModel.ToProductOutputModel(
                    _dummyProducts[0],
                    _dummyBrands[0],
                    _dummyCategories[0]
                )
            );
    }

    [Fact]
    public async Task AddProductAsync_ShouldReturnIdOutputModel()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(_dummyProducts[0].Id))
            .ReturnsAsync((Product?)null);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(_dummyProducts[0].CategoryId))
            .ReturnsAsync(_dummyCategories[0]);

        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<IServiceError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Success<IServiceError, IdOutputModel>(
                new IdOutputModel(1))
            );

        // Act
        var productResult = await _productService.AddProductAsync(
            _dummyProducts[0].Id,
            _dummyProducts[0].Name,
            _dummyProducts[0].ImageUrl,
            _dummyProducts[0].Quantity,
            _dummyProducts[0].Unit,
            _dummyBrands[0].Name,
            _dummyProducts[0].CategoryId
        );

        // Assert
        productResult
            .Value
            .Should()
            .BeEquivalentTo(new IdOutputModel(_dummyProducts[0].Id));
    }
}