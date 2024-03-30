using FluentAssertions;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi_test.Application.Repository;

public class ProductRepositoryTest
{
    [Fact]
    public async Task GetProductByIdAsync_ReturnsObjectAsync()
    {
        // Arrange
        var expectedProduct = new ProductEntity
        {
            Id = 123,
            Name = "Filipinos",
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
    public async Task AddProductAsync_ReturnsObjectAsync()
    {
        // Arrange
        var expectedProduct = new ProductEntity
        {
            Id = 123,
            Name = "Filipinos",
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
            expectedProduct.Id,
            expectedProduct.Name,
            expectedProduct.ImageUrl,
            expectedProduct.Quantity,
            expectedProduct.Unit,
            expectedProduct.BrandId,
            expectedProduct.CategoryId
        );

        // Assert
        actualProduct.Should().Be(expectedProduct.Id);
    }

    [Fact]
    public async Task GetProductsAsync_ReturnsListAsync()
    {
        // Arrange
        var expectedProducts = new List<ProductEntity>
        {
            new()
            {
                Id = 1,
                Name = "Filipinos",
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
    public async Task GetProductsByCategoryIdAsync_ReturnsListAsync()
    {
        // Arrange
        var expectedProducts = new List<ProductEntity>
        {
            new()
            {
                Id = 1,
                Name = "Filipinos",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 1,
                CategoryId = 12
            },
            new()
            {
                Id = 2,
                Name = "Gomas",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 3,
                CategoryId = 12
            },
            new()
            {
                Id = 3,
                Name = "Maça",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 2,
                CategoryId = 13
            }
        };

        var context = DbHelper.CreateDatabase(expectedProducts);
        var productRepo = new ProductRepository(context);

        // Act
        var actualProducts = await productRepo.GetProductsAsync(brandId: 12);

        // Assert
        actualProducts.Should().BeEquivalentTo(expectedProducts.Where(p => p.CategoryId == 12));
    }
    
    [Fact]
    public async Task GetProductsByBrandIdAsync_ReturnsListAsync()
    {
        // Arrange
        var expectedProducts = new List<ProductEntity>
        {
            new()
            {
                Id = 1,
                Name = "Filipinos",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 1,
                CategoryId = 12
            },
            new()
            {
                Id = 2,
                Name = "Gomas",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 3,
                CategoryId = 12
            },
            new()
            {
                Id = 3,
                Name = "Maça",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 2,
                CategoryId = 13
            }
        };

        var context = DbHelper.CreateDatabase(expectedProducts);
        var productRepo = new ProductRepository(context);

        // Act
        var actualProducts = await productRepo.GetProductsAsync(brandId: 3);

        // Assert
        actualProducts.Should().BeEquivalentTo(expectedProducts.Where(p => p.BrandId == 3));
    }
    
    [Fact]
    public async Task GetProductsByCategoryIdAndBrandIdAsync_ReturnsListAsync()
    {
        // Arrange
        var expectedProducts = new List<ProductEntity>
        {
            new()
            {
                Id = 1,
                Name = "Filipinos",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 1,
                CategoryId = 12
            },
            new()
            {
                Id = 2,
                Name = "Gomas",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 3,
                CategoryId = 12
            },
            new()
            {
                Id = 3,
                Name = "Maça",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 2,
                CategoryId = 13
            }
        };

        var context = DbHelper.CreateDatabase(expectedProducts);
        var productRepo = new ProductRepository(context);

        // Act
        var actualProducts = await productRepo.GetProductsAsync(brandId: 3, categoryId: 12);

        // Assert
        actualProducts.Should().BeEquivalentTo(expectedProducts.Where(p => p.BrandId == 3 && p.CategoryId == 12));
    }
    
    [Fact]
    public async Task GetProcutsByNameAsync_ReturnsListAsync()
    {
        // Arrange
        var expectedProducts = new List<ProductEntity>
        {
            new()
            {
                Id = 1,
                Name = "Filipinos",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 1,
                CategoryId = 12
            },
            new()
            {
                Id = 2,
                Name = "Gomas",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 3,
                CategoryId = 12
            },
            new()
            {
                Id = 3,
                Name = "Maça",
                ImageUrl = "dummy_image_url",
                Quantity = 1,
                Unit = "unidades",
                BrandId = 2,
                CategoryId = 13
            }
        };

        var context = DbHelper.CreateDatabase(expectedProducts);
        var productRepo = new ProductRepository(context);

        // Act
        var actualProducts = await productRepo.GetProductsAsync(name: "Gomas");

        // Assert
        // TODO: Fix this test
    }
}