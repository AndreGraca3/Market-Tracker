using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Repository.Market.Inventory.Brand;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;

namespace market_tracker_webapi_test.Application.Repository.Market.Inventory;

public class BrandRepositoryTest
{
    [Fact]
    public async Task GetBrandByIdAsync_WhenBrandExists_ReturnsBrand()
    {
        // Arrange
        var brandEntities = new List<BrandEntity>
        {
            new BrandEntity { Id = 1, Name = "brandName" },
            new BrandEntity { Id = 2, Name = "brandName2" }
        };
        
        var context = DbHelper.CreateDatabase(brandEntities);
        
        var brandRepository = new BrandRepository(context);
        
        // Act
        var actualBrand = await brandRepository.GetBrandByIdAsync(1);
        
        // Assert
        var expectedBrand = new Brand(1, "brandName");
        actualBrand.Should().BeEquivalentTo(expectedBrand);
    }
    
    [Fact]
    public async Task GetBrandByIdAsync_WhenBrandDoesNotExist_ReturnsNull()
    {
        // Arrange
        var brandEntities = new List<BrandEntity>
        {
            new BrandEntity { Id = 1, Name = "brandName" },
            new BrandEntity { Id = 2, Name = "brandName2" }
        };
        
        var context = DbHelper.CreateDatabase(brandEntities);
        
        var brandRepository = new BrandRepository(context);
        
        // Act
        var actualBrand = await brandRepository.GetBrandByIdAsync(3);
        
        // Assert
        actualBrand.Should().BeNull();
    }
    
    [Fact]
    public async Task GetBrandByNameAsync_WhenBrandExists_ReturnsBrand()
    {
        // Arrange
        var brandEntities = new List<BrandEntity>
        {
            new BrandEntity { Id = 1, Name = "brandName" },
            new BrandEntity { Id = 2, Name = "brandName2" }
        };
        
        var context = DbHelper.CreateDatabase(brandEntities);
        
        var brandRepository = new BrandRepository(context);
        
        // Act
        var actualBrand = await brandRepository.GetBrandByNameAsync("brandName");
        
        // Assert
        var expectedBrand = new Brand(1, "brandName");
        actualBrand.Should().BeEquivalentTo(expectedBrand);
    }
    
    [Fact]
    public async Task GetBrandByNameAsync_WhenBrandDoesNotExist_ReturnsNull()
    {
        // Arrange
        var brandEntities = new List<BrandEntity>
        {
            new BrandEntity { Id = 1, Name = "brandName" },
            new BrandEntity { Id = 2, Name = "brandName2" }
        };
        
        var context = DbHelper.CreateDatabase(brandEntities);
        
        var brandRepository = new BrandRepository(context);
        
        // Act
        var actualBrand = await brandRepository.GetBrandByNameAsync("brandName3");
        
        // Assert
        actualBrand.Should().BeNull();
    }
    
    [Fact]
    public async Task AddBrandAsync_WhenBrandDoesNotExist_AddsBrand()
    {
        // Arrange
        var brandEntities = new List<BrandEntity>
        {
            new BrandEntity { Id = 1, Name = "brandName" },
            new BrandEntity { Id = 2, Name = "brandName2" }
        };
        
        var context = DbHelper.CreateDatabase(brandEntities);
        
        var brandRepository = new BrandRepository(context);
        
        // Act
        var actualBrand = await brandRepository.AddBrandAsync("brandName3");
        
        // Assert
        var expectedBrand = new Brand(3, "brandName3");
        var expectedBrandEntity = new BrandEntity { Id = 3, Name = "brandName3" };
        
        actualBrand.Should().BeEquivalentTo(expectedBrand);
        context.Brand.Should().ContainEquivalentOf(expectedBrandEntity);
    }
    
    [Fact]
    public async Task RemoveBrandAsync_WhenBrandExists_RemovesBrand()
    {
        // Arrange
        var brandEntities = new List<BrandEntity>
        {
            new BrandEntity { Id = 1, Name = "brandName" },
            new BrandEntity { Id = 2, Name = "brandName2" }
        };
        
        var context = DbHelper.CreateDatabase(brandEntities);
        
        var brandRepository = new BrandRepository(context);
        
        // Act
        var actualBrand = await brandRepository.RemoveBrandAsync(1);
        
        // Assert
        var expectedBrand = new Brand(1, "brandName");
        var expectedBrandEntity = new BrandEntity { Id = 1, Name = "brandName" };
        
        actualBrand.Should().BeEquivalentTo(expectedBrand);
        context.Brand.Should().NotContainEquivalentOf(expectedBrandEntity);
    }
}