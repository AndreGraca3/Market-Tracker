using market_tracker_webapi.Application.Domain.Models.Market.Inventory;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Inventory.Category;

using Category = Domain.Models.Market.Inventory.Category;

public interface ICategoryService
{
    public Task<Either<IServiceError, IEnumerable<Category>>> GetCategoriesAsync();

    public Task<Either<CategoryFetchingError, Category>> GetCategoryAsync(int id);

    public Task<Either<ICategoryError, CategoryId>> AddCategoryAsync(string name);

    public Task<Either<ICategoryError, Category>> UpdateCategoryAsync(int id, string name);

    public Task<Either<CategoryFetchingError, CategoryId>> RemoveCategoryAsync(int id);
}
