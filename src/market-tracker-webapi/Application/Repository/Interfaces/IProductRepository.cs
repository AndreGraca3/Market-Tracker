using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(); // TODO: Add filter, pagination, sorting and search

        Task<Product?> GetProductByIdAsync(int productId);

        Task<int> AddProductAsync(
            int id,
            string name,
            string description,
            string imageUrl,
            int quantity,
            string unit,
            int brandId,
            int categoryId
        );

        Task<Product> UpdateProductAsync(
            int productId,
            float? price = null,
            string? description = null,
            string? imageUrl = null,
            int? quantity = null,
            string? unit = null,
            int? brandId = null,
            int? categoryId = null,
            int? rate = null
        );

        Task<Product?> RemoveProductAsync(int productId);

        Task<List<ProductReview>> GetReviewsByProductIdAsync(int productId);

        Task<int> AddReviewAsync(Guid clientId, int productId, int rate, string comment);

        Task<ProductReview> UpdateReviewAsync(
            Guid clientId,
            int productId,
            int rate,
            string comment
        );

        Task RemoveReviewAsync(Guid clientId, int productId);
    }
}
