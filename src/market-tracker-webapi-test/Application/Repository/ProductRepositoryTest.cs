using FluentAssertions;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi_test.Application.Repository;

public class ProductRepositoryTest
{
    [Fact]
    public async void GetProductByIdAsync_ReturnsObjectAsync()
    {
        // Arrange
        var expectedProduct = new ProductEntity
        {
            Id = 123,
            Name = "Filipinos",
            Description = "Deliciosos e achocolatados",
            ImageUrl = "dummy_image_url",
            Quantity = 1,
            Unit = "unidades",
            BrandId = 1,
            CategoryId = 12
        };

        var mockedEntities = new List<ProductEntity>
        {
            expectedProduct,
            new()
            {
                Id = 2,
                Name = "Maça",
                Description = "Fresca e deliciosa",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 2,
                CategoryId = 9
            },
            new()
            {
                Id = 3,
                Name = "Gomas",
                Description = "Açucaradas e coloridas",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 3,
                CategoryId = 5
            }
        };

        var context = DbHelper.CreateDatabase(mockedEntities);
        var productRepo = new ProductRepository(context);

        // Act
        var actualProduct = await productRepo.GetProductByIdAsync(expectedProduct.Id);

        // Assert
        actualProduct.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async void AddProductAsync_ReturnsObjectAsync()
    {
        // Arrange
        var expectedCategory = new ProductEntity
        {
            Id = 123,
            Name = "Filipinos",
            Description = "Deliciosos e achocolatados",
            ImageUrl = "dummy_image_url",
            Quantity = 1,
            Unit = "unidades",
            BrandId = 1,
            CategoryId = 12
        };

        var mockedEntities = new List<ProductEntity>
        {
            new()
            {
                Id = 1,
                Name = "Maça",
                Description = "Fresca e deliciosa",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 2,
                CategoryId = 9
            },
            new()
            {
                Id = 2,
                Name = "Gomas",
                Description = "Açucaradas e coloridas",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 3,
                CategoryId = 5
            }
        };

        var context = DbHelper.CreateDatabase(mockedEntities);
        var productRepo = new ProductRepository(context);

        // Act
        var actualProduct = await productRepo.AddProductAsync(
            expectedCategory.Id,
            expectedCategory.Name,
            expectedCategory.Description,
            expectedCategory.ImageUrl,
            expectedCategory.Quantity,
            expectedCategory.Unit,
            expectedCategory.BrandId,
            expectedCategory.CategoryId
        );

        // Assert
        actualProduct.Should().Be(expectedCategory.Id);
    }

    [Fact]
    public async void GetProductsAsync_ReturnsListAsync()
    {
        // Arrange
        var expectedProducts = new List<ProductEntity>
        {
            new()
            {
                Id = 1,
                Name = "Filipinos",
                Description = "Deliciosos e achocolatados",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 1,
                CategoryId = 12
            },
            new()
            {
                Id = 2,
                Name = "Maça",
                Description = "Fresca e deliciosa",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 2,
                CategoryId = 9
            },
        };

        var context = DbHelper.CreateDatabase(expectedProducts);
        var productRepo = new ProductRepository(context);

        // Act
        var actualProducts = await productRepo.GetProductsAsync();

        // Assert
        actualProducts.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public async void GetProductsByCategoryIdAsync_ReturnsListAsync()
    {
        // Arrange
        var expectedProducts = new List<ProductEntity>
        {
            new()
            {
                Id = 1,
                Name = "Filipinos",
                Description = "Deliciosos e achocolatados",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 1,
                CategoryId = 12
            },
            new()
            {
                Id = 2,
                Name = "Maça",
                Description = "Fresca e deliciosa",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 2,
                CategoryId = 12
            },
        };

        var context = DbHelper.CreateDatabase(expectedProducts);
        var productRepo = new ProductRepository(context);

        // Act
        var actualProducts = await productRepo.GetProductsAsync();
    }
}
