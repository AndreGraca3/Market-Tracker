using FluentAssertions;
using market_tracker_webapi.Application.Repository.Operations.Category;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi_test.Application.Repository;

public class CategoryRepositoryTest
{
    [Fact]
    public async void GetCategoryAsync_ReturnsObjectAsync()
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
    public async void GetCategoryAsync_ReturnsNullAsync()
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
    public async void GetCategoriesAsync_ReturnsListAsync()
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
    public async void GetCategoriesAsync_ReturnsEmptyListAsync()
    {
        // Arrange
        var context = DbHelper.CreateDatabase<CategoryEntity>([]);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategories = await categoryRepo.GetCategoriesAsync();

        // Assert
        actualCategories.Should().BeEmpty();
    }

    [Fact]
    public async void AddCategoryAsync_ReturnsObjectAsync()
    {
        // Arrange
        var expectedCategory = new CategoryEntity { Id = 1, Name = "Talho" };

        var context = DbHelper.CreateDatabase<CategoryEntity>([]);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.AddCategoryAsync(expectedCategory.Name);

        // Assert
        actualCategory.Should().Be(expectedCategory.Id);
    }

    [Fact]
    public async void UpdateCategoryAsync_ReturnsObjectAsync()
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
    public async void DeleteCategoryAsync_ReturnsObjectAsync()
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
    public async void DeleteCategoryAsync_ReturnsNullAsync()
    {
        // Arrange
        var context = DbHelper.CreateDatabase<CategoryEntity>([]);
        var categoryRepo = new CategoryRepository(context);

        // Act
        var actualCategory = await categoryRepo.RemoveCategoryAsync(1);

        // Assert
        actualCategory.Should().BeNull();
    }
}
