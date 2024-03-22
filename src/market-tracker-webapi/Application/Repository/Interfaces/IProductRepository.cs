using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductEntity?> GetProductAsync(int productId);

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

        Task<IdOutputModel> UpdateProductAsync(
            int productId,
            float? price = null,
            string? description = null
        );

        Task<ProductEntity> DeleteProductAsync(ProductEntity product);

        Task<List<IdOutputModel>> GetProductsAsync(
            string filter,
            Pagination pagination
        );

        Task<BrandModel> GetBrandAsync(int brandId);

        Task<int> AddBrandAsync(string name);

        Task DeleteBrandAsync(int brandId);

        Task<List<CommentModel>> GetReviewFromProduct(int commentId);

        Task<int> CreateReviewAsync(int productId, int userId, CommentModel comment);

        Task<CommentModel> UpdateReviewAsync(int commentId);

        Task DeleteReviewAsync(int commentId);
    }
}
