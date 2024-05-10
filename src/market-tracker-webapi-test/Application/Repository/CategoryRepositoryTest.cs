using FluentAssertions;
using market_tracker_webapi.Application.Repository.Market.Inventory.Category;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;

namespace market_tracker_webapi_test.Application.Repository;

public class CategoryRepositoryTest
{
    [Fact]
    public async Task GetCategoryByIdAsync_ReturnsObjectAsync()
    {
        // Arrange
        var expectedCategory = new CategoryEntity { Id = 1, Name = "Talho" };

        var mockedEntities = new List<CategoryEntity>
        {
            expectedCategory,
            new() { Id = 2, Name = "Legumes" },
            new() { Id = 3, Name = "Mercearia" }
        };

        var context = DbHelper.CreateDatabase(mockedEntities);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.GetCategoryByIdAsync(1);

        // Assert
        actualCategory.Should().BeEquivalentTo(expectedCategory);
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
        var expectedCategory = new CategoryEntity { Id = 1, Name = "Talho" };

        var mockedEntities = new List<CategoryEntity>
        {
            expectedCategory,
            new() { Id = 2, Name = "Legumes" },
            new() { Id = 3, Name = "Mercearia" }
        };

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
        var expectedCategories = new List<CategoryEntity>
        {
            new() { Id = 1, Name = "Talho" },
            new() { Id = 2, Name = "Legumes" },
            new() { Id = 3, Name = "Mercearia" }
        };

        var context = DbHelper.CreateDatabase(expectedCategories);
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
        actualCategory.Should().Be(expectedCategory.Id);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ReturnsObjectAsync()
    {
        // Arrange
        var expectedCategory = new CategoryEntity { Id = 1, Name = "Talho" };

        var context = DbHelper.CreateDatabase([expectedCategory]);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.UpdateCategoryAsync(
            expectedCategory.Id,
            "Peixaria"
        );

        // Assert
        actualCategory.Should().BeEquivalentTo(new CategoryEntity { Id = 1, Name = "Peixaria" });
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
        var expectedCategory = new CategoryEntity { Id = 1, Name = "Talho" };

        var context = DbHelper.CreateDatabase([expectedCategory]);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.RemoveCategoryAsync(expectedCategory.Id);

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