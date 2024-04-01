using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Service.Errors.Category;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Category;

using Category = market_tracker_webapi.Application.Domain.Category;

public interface ICategoryService
{
    public Task<EnumerableOutputModel> GetCategoriesAsync();

    public Task<Either<CategoryFetchingError, Category>> GetCategoryAsync(int id);

    public Task<Either<ICategoryError, IdOutputModel>> AddCategoryAsync(string name);

    public Task<Either<ICategoryError, IdOutputModel>> UpdateCategoryAsync(int id, string name);

    public Task<Either<CategoryFetchingError, IdOutputModel>> RemoveCategoryAsync(int id);
}
