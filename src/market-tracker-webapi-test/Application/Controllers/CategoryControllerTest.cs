using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Http.Controllers.Market.Inventory;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Category;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Category;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Application.Controllers;

public class CategoryControllerTest
{
    private readonly Mock<ICategoryService> _categoryServiceMock = new();
    private readonly CategoryController _categoryController;

    public CategoryControllerTest()
    {
        _categoryController = new CategoryController(_categoryServiceMock.Object);
    }
    
    [Fact]
    public async Task GetCategories_ReturnsOk()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category(1, "Category 1"),
            new Category(2, "Category 2")
        };
        _categoryServiceMock
            .Setup(x => x.GetCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _categoryController.GetCategoriesAsync();

        // Assert
        result.Value.Should().BeEquivalentTo(categories.Select(c => c.ToOutputModel()).ToCollectionOutputModel());
    }
    
    [Fact]
    public async Task GetCategory_ReturnsOk()
    {
        // Arrange
        var category = new Category(1, "Category 1");
        _categoryServiceMock
            .Setup(x => x.GetCategoryAsync(1))
            .ReturnsAsync(category);

        // Act
        var result = await _categoryController.GetCategoryAsync(1);

        // Assert
        result.Value.Should().BeEquivalentTo(category.ToOutputModel());
    }

    [Fact]
    public async Task AddCategory_ReturnsCreated()
    {
        // Arrange
        var categoryInput = new CategoryInputModel("Category 1");
        var categoryId = new CategoryId(1);
        
        _categoryServiceMock
            .Setup(x => x.AddCategoryAsync("Category 1"))
            .ReturnsAsync(categoryId);
        
        // Act
        var result = await _categoryController.AddCategoryAsync(categoryInput);
        
        // Assert
        result.Result.Should().BeOfType<CreatedResult>();
    }
    
    [Fact]
    public async Task UpdateCategory_ReturnsOk()
    {
        // Arrange
        var categoryInput = new CategoryInputModel("Category 1");
        var category = new Category(1, "Category 1");
        
        _categoryServiceMock
            .Setup(x => x.UpdateCategoryAsync(1, "Category 1"))
            .ReturnsAsync(category);
        
        // Act
        var result = await _categoryController.UpdateCategoryAsync(1, categoryInput);
        
        // Assert
        result.Value.Should().BeEquivalentTo(category.ToOutputModel());
    }
    
    [Fact]
    public async Task RemoveCategory_ReturnsNoContent()
    {
        // Arrange
        _categoryServiceMock
            .Setup(x => x.RemoveCategoryAsync(1))
            .ReturnsAsync(new CategoryId(1));
        
        // Act
        var result = await _categoryController.RemoveCategoryAsync(1);
        
        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}