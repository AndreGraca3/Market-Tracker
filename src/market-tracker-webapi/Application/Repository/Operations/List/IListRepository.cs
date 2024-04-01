using market_tracker_webapi.Application.Http.Models;

namespace market_tracker_webapi.Application.Repository.Operations.List   
{
    public interface IListRepository
    {
        Task<Models.ListData> GetCartAsync(int id);

        Task<int> CreateCartAsync(IdOutputModel productCreationOutputModel);

        Task<int> UpdateCartAsync(int id, IdOutputModel productCreationOutputModel);

        Task<int> DeleteCartAsync(int id);

        Task<List<Models.ListData>> GetCartsFromUserAsync(int userId);
    }
}
