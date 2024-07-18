using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Repository.Market.Inventory.Category;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Category;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class CategoryServiceTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    
    private readonly ICategoryService _categoryService;
    
    public CategoryServiceTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock.Object, new MockedTransactionManager());
    }
    
    [Fact]
    public async Task GetCategoriesAsync_ShouldReturnCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new(1, "Category 1"),
            new(2, "Category 2")
        };
        
        _categoryRepositoryMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(categories);
        
        // Act
        var result = await _categoryService.GetCategoriesAsync();
        
        // Assert
        result.Should().BeEquivalentTo(categories);
    }
    
    [Fact]
    public async Task GetCategoryAsync_ShouldReturnCategory()
    {
        // Arrange
        var category = new Category(1, "Category 1");
        
        _categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(1)).ReturnsAsync(category);
        
        // Act
        var result = await _categoryService.GetCategoryAsync(1);
        
        // Assert
        result.Should().BeEquivalentTo(category);
    }
    
    [Fact]
    public async Task GetCategoryAsync_ShouldThrowException_WhenCategoryNotFound()
    {
        // Arrange
        _categoryRepositoryMock.Setup(x => x.GetCategoryByIdAsync(1)).ReturnsAsync((Category)null);
        
        // Act
        Func<Task> act = async () => await _categoryService.GetCategoryAsync(1);
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
    
    [Fact]
    public async Task AddCategoryAsync_ShouldReturnCategoryId()
    {
        // Arrange
        var categoryId = new CategoryId(1);
        
        _categoryRepositoryMock.Setup(x => x.GetCategoryByNameAsync("Category 1")).ReturnsAsync((Category)null);
        _categoryRepositoryMock.Setup(x => x.AddCategoryAsync("Category 1")).ReturnsAsync(categoryId);
        
        // Act
        var result = await _categoryService.AddCategoryAsync("Category 1");
        
        // Assert
        result.Should().BeEquivalentTo(categoryId);
    }
    
    [Fact]
    public async Task AddCategoryAsync_ShouldThrowException_WhenCategoryNameAlreadyExists()
    {
        // Arrange
        _categoryRepositoryMock.Setup(x => x.GetCategoryByNameAsync("Category 1")).ReturnsAsync(new Category(1, "Category 1"));
        
        // Act
        Func<Task> act = async () => await _categoryService.AddCategoryAsync("Category 1");
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_ShouldReturnCategory()
    {
        // Arrange
        var category = new Category(1, "Category 1");
        
        _categoryRepositoryMock.Setup(x => x.GetCategoryByNameAsync("Category 1")).ReturnsAsync((Category)null);
        _categoryRepositoryMock.Setup(x => x.UpdateCategoryAsync(1, "Category 1")).ReturnsAsync(category);
        
        // Act
        var result = await _categoryService.UpdateCategoryAsync(1, "Category 1");
        
        // Assert
        result.Should().BeEquivalentTo(category);
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_ShouldThrowException_WhenCategoryNameAlreadyExists()
    {
        // Arrange
        _categoryRepositoryMock.Setup(x => x.GetCategoryByNameAsync("Category 1")).ReturnsAsync(new Category(1, "Category 1"));
        
        // Act
        Func<Task> act = async () => await _categoryService.UpdateCategoryAsync(1, "Category 1");
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_ShouldThrowException_WhenCategoryNotFound()
    {
        // Arrange
        _categoryRepositoryMock.Setup(x => x.GetCategoryByNameAsync("Category 1")).ReturnsAsync((Category)null);
        _categoryRepositoryMock.Setup(x => x.UpdateCategoryAsync(1, "Category 1")).ReturnsAsync((Category)null);
        
        // Act
        Func<Task> act = async () => await _categoryService.UpdateCategoryAsync(1, "Category 1");
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
    
    [Fact]
    public async Task RemoveCategoryAsync_ShouldReturnCategoryId()
    {
        // Arrange
        var category = new Category(1, "Category 1");
        
        _categoryRepositoryMock.Setup(x => x.RemoveCategoryAsync(1)).ReturnsAsync(category);
        
        // Act
        var result = await _categoryService.RemoveCategoryAsync(1);
        
        // Assert
        result.Should().BeEquivalentTo(category.Id);
    }
    
    [Fact]
    public async Task RemoveCategoryAsync_ShouldThrowException_WhenCategoryNotFound()
    {
        // Arrange
        _categoryRepositoryMock.Setup(x => x.RemoveCategoryAsync(1)).ReturnsAsync((Category)null);
        
        // Act
        Func<Task> act = async () => await _categoryService.RemoveCategoryAsync(1);
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
}