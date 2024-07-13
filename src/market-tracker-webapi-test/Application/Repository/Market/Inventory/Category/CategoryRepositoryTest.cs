using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Repository.Market.Inventory.Category;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;

namespace market_tracker_webapi_test.Application.Repository;

public class CategoryRepositoryTest
{
    [Fact]
    public async Task GetCategoryByIdAsync_ReturnsObjectAsync()
    {
        // Arrange

        var mockedEntities = new List<CategoryEntity>
        {
            new() { Id = 1, Name = "Talho" },
            new() { Id = 2, Name = "Legumes" },
            new() { Id = 3, Name = "Mercearia" }
        };
        
        var expectedCategoryObj = new Category(1, "Talho");

        var context = DbHelper.CreateDatabase(mockedEntities);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.GetCategoryByIdAsync(1);

        // Assert
        actualCategory.Should().BeEquivalentTo(expectedCategoryObj);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ReturnsNullAsync()
    {
        // Arrange
        var mockedEntities = new List<CategoryEntity>
        {
            new() { Id = 1, Name = "Talho" },
            new() { Id = 2, Name = "Legumes" },
            new() { Id = 3, Name = "Mercearia" }
        };

        var context = DbHelper.CreateDatabase(mockedEntities);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.GetCategoryByIdAsync(4);

        // Assert
        actualCategory.Should().BeNull();
    }

    [Fact]
    public async Task GetCategoryByNameAsync_ReturnsObjectAsync()
    {
        // Arrange

        var mockedEntities = new List<CategoryEntity>
        {
            new() { Id = 1, Name = "Talho" },
            new() { Id = 2, Name = "Legumes" },
            new() { Id = 3, Name = "Mercearia" }
        };
        
        var expectedCategory = new Category(1, "Talho");

        var context = DbHelper.CreateDatabase(mockedEntities);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.GetCategoryByNameAsync("Talho");

        // Assert
        actualCategory.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_ReturnsNullAsync()
    {
        // Arrange
        var mockedEntities = new List<CategoryEntity>
        {
            new() { Id = 1, Name = "Talho" },
            new() { Id = 2, Name = "Legumes" },
            new() { Id = 3, Name = "Mercearia" }
        };

        var context = DbHelper.CreateDatabase(mockedEntities);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.GetCategoryByNameAsync("Peixaria");

        // Assert
        actualCategory.Should().BeNull();
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsListAsync()
    {
        // Arrange
        var categoryEntities = new List<CategoryEntity>
        {
            new() { Id = 1, Name = "Talho" },
            new() { Id = 2, Name = "Legumes" },
            new() { Id = 3, Name = "Mercearia" }
        };
        
        var expectedCategories = new List<Category>
        {
            new(1, "Talho"),
            new(2, "Legumes"),
            new(3, "Mercearia")
        };

        var context = DbHelper.CreateDatabase(categoryEntities);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategories = await categoryRepo.GetCategoriesAsync();

        // Assert
        actualCategories.Should().BeEquivalentTo(expectedCategories);
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsEmptyListAsync()
    {
        // Arrange
        var context = DbHelper.CreateDatabase();
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategories = await categoryRepo.GetCategoriesAsync();

        // Assert
        actualCategories.Should().BeEmpty();
    }

    [Fact]
    public async Task AddCategoryAsync_ReturnsObjectAsync()
    {
        // Arrange
        var expectedCategory = new CategoryEntity { Id = 1, Name = "Talho" };

        var context = DbHelper.CreateDatabase();
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.AddCategoryAsync(expectedCategory.Name);

        // Assert
        actualCategory.Value.Should().Be(expectedCategory.Id);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ReturnsObjectAsync()
    {
        // Arrange
        var categoryEntity = new CategoryEntity { Id = 1, Name = "Talho" };

        var context = DbHelper.CreateDatabase([categoryEntity]);
        var categoryRepo = new CategoryRepository(context);

        var expectedCategory = new Category(Id: 1, Name: "Peixaria");

        // Act
        var actualCategory = await categoryRepo.UpdateCategoryAsync(
            categoryEntity.Id,
            "Peixaria"
        );

        // Assert
        actualCategory.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ReturnsNullAsync()
    {
        // Arrange
        var context = DbHelper.CreateDatabase();
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.UpdateCategoryAsync(1, "Peixaria");

        // Assert
        actualCategory.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCategoryAsync_ReturnsObjectAsync()
    {
        // Arrange
        var categoryEntity = new CategoryEntity { Id = 1, Name = "Talho" };
        var expectedCategory = new Category(Id: 1, Name: "Talho");

        var context = DbHelper.CreateDatabase([categoryEntity]);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.RemoveCategoryAsync(categoryEntity.Id);

        // Assert
        actualCategory.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async Task DeleteCategoryAsync_ReturnsNullAsync()
    {
        // Arrange
        var context = DbHelper.CreateDatabase();
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.RemoveCategoryAsync(1);

        // Assert
        actualCategory.Should().BeNull();
    }
}