using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Repository.Operations.Brand;
using market_tracker_webapi.Application.Repository.Operations.Category;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Operations.Product;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class ProductServiceTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IBrandRepository> _brandRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

    private readonly ProductService _productService;

    public ProductServiceTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _productService = new ProductService(
            _productRepositoryMock.Object,
            _brandRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            new MockedTransactionManager()
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

    [Fact]
    public async Task GetProductsAsync_ShouldReturnProducts()
    {
        // Arrange
        _productRepositoryMock.Setup(repo => repo.GetProductsAsync()).ReturnsAsync(_dummyProducts);

        _brandRepositoryMock
            .Setup(repo => repo.GetBrandByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_dummyBrands[0]);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_dummyCategories[0]);

        // Act
        var productsResult = await _productService.GetProductsAsync();

        // Assert
        productsResult.Should().BeEquivalentTo(new CollectionOutputModel(_dummyProducts));
    }

    [Fact]
    public async Task GetProductAsync_ShouldReturnProduct()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_dummyProducts[0]);

        _brandRepositoryMock
            .Setup(repo => repo.GetBrandByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_dummyBrands[0]);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_dummyCategories[0]);

        // Act
        var productResult = await _productService.GetProductByIdAsync(It.IsAny<int>());

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
    public async Task GetProductAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        // Act
        var productResult = await _productService.GetProductByIdAsync(It.IsAny<int>());

        // Assert
        productResult
            .Error.Should()
            .BeEquivalentTo(new ProductFetchingError.ProductByIdNotFound(It.IsAny<int>()));
    }

    [Fact]
    public async Task AddProductAsync_ShouldReturnIdOutputModel()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(_dummyProducts[0].Id))
            .ReturnsAsync((Product?)null);

        _productRepositoryMock
            .Setup(repo =>
                repo.AddProductAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )
            )
            .ReturnsAsync(It.IsAny<int>());

        _brandRepositoryMock
            .Setup(repo => repo.GetBrandByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new Brand(It.IsAny<int>(), It.IsAny<string>()));

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Category(It.IsAny<int>(), It.IsAny<string>()));

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
        productResult.Value.Should().BeEquivalentTo(new IntIdOutputModel(_dummyProducts[0].Id));
    }

    [Fact]
    public async Task AddProductAsync_ShouldReturnProductAlreadyExists()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(_dummyProducts[0].Id))
            .ReturnsAsync(_dummyProducts[0]);

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
            .Error.Should()
            .BeEquivalentTo(new ProductCreationError.ProductAlreadyExists(_dummyProducts[0].Id));
    }

    [Fact]
    public async Task AddProductAsync_ShouldReturnCategoryByIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(_dummyProducts[0].Id))
            .ReturnsAsync((Product?)null);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(_dummyProducts[0].CategoryId))
            .ReturnsAsync((Category?)null);

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
            .Error.Should()
            .BeEquivalentTo(
                new CategoryFetchingError.CategoryByIdNotFound(_dummyProducts[0].CategoryId)
            );
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldReturnProductOutputModel()
    {
        // Arrange
        var expectedNewProduct = new Product(
            _dummyProducts[0].Id,
            _dummyProducts[1].Name,
            _dummyProducts[1].ImageUrl,
            _dummyProducts[1].Quantity,
            _dummyProducts[1].Unit,
            _dummyProducts[0].Views,
            _dummyProducts[0].Rating,
            _dummyProducts[1].BrandId,
            _dummyProducts[1].CategoryId
        );

        _brandRepositoryMock
            .Setup(repo => repo.GetBrandByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(_dummyBrands[1]);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_dummyCategories[1]);

        _productRepositoryMock
            .Setup(repo =>
                repo.UpdateProductAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )
            )
            .ReturnsAsync(expectedNewProduct);

        // Act
        var productResult = await _productService.UpdateProductAsync(
            _dummyProducts[0].Id,
            _dummyProducts[1].Name,
            _dummyProducts[1].ImageUrl,
            _dummyProducts[1].Quantity,
            _dummyProducts[1].Unit,
            _dummyBrands[1].Name,
            _dummyProducts[1].CategoryId
        );

        // Assert
        productResult
            .Value.Should()
            .BeEquivalentTo(
                ProductOutputModel.ToProductOutputModel(
                    expectedNewProduct,
                    _dummyBrands[1],
                    _dummyCategories[1]
                )
            );
    }

    [Fact]
    public async Task RemoveProductAsync_ShouldReturnIdOutputModel()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_dummyProducts[0]);

        _productRepositoryMock
            .Setup(repo => repo.RemoveProductAsync(It.IsAny<int>()))
            .ReturnsAsync(_dummyProducts[0]);

        // Act
        var productResult = await _productService.RemoveProductAsync(_dummyProducts[0].Id);

        // Assert
        productResult.Value.Should().BeEquivalentTo(new IntIdOutputModel(_dummyProducts[0].Id));
    }

    [Fact]
    public async Task RemoveProductAsync_ShouldReturnProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        // Act
        var productResult = await _productService.RemoveProductAsync(_dummyProducts[0].Id);

        // Assert
        productResult
            .Error.Should()
            .BeEquivalentTo(new ProductFetchingError.ProductByIdNotFound(_dummyProducts[0].Id));
    }
}
