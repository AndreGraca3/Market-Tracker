﻿namespace market_tracker_webapi.Application.Repository.Operations.List   
{
    public interface IListRepository
    {
        Task<Models.ListData> GetCartAsync(int id);

        Task<int> CreateCartAsync(Models.ProductData productData);

        Task<int> UpdateCartAsync(int id, Models.ProductData productData);

        Task<int> DeleteCartAsync(int id);

        Task<List<Models.ListData>> GetCartsFromUserAsync(int userId);
    }
}