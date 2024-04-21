using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Category;

using Category = market_tracker_webapi.Application.Domain.Category;

public interface ICategoryService
{
    public Task<Either<IServiceError, CollectionOutputModel>> GetCategoriesAsync();

    public Task<Either<CategoryFetchingError, Category>> GetCategoryAsync(int id);

    public Task<Either<ICategoryError, IntIdOutputModel>> AddCategoryAsync(string name);

    public Task<Either<ICategoryError, Category>> UpdateCategoryAsync(int id, string name);

    public Task<Either<CategoryFetchingError, IntIdOutputModel>> RemoveCategoryAsync(int id);
}
