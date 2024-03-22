using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;

namespace market_tracker_webapi.Application.Repository.Interfaces
{
    public interface IClientRepository
    {
        Task FavoriteProductAsync(int id);

        Task<PostData> GetRecipeAsync(int recipeId);
        
        Task<int> CreateRecipeAsync(List<IdOutputModel> products, string text);
    }
}