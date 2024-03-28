using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;

namespace market_tracker_webapi.Application.Repositories.Product
{
    public interface IProductRepository
    {
        Task<ProductData> GetProductAsync(int productId);

        Task<int> AddProductAsync(int productId, string description, string imageUrl, int quantity, float price, string unit);

        Task<ProductData> UpdateProductAsync(int productId, float? price = null, string? description = null);

        Task DeleteProductAsync(int productId);

        Task<List<ProductData>> GetProductsAsync(string filter, Pagination pagination);

        Task<BrandData> GetBrandAsync(int brandId);

        Task<int> AddBrandAsync(string name);

        Task DeleteBrandAsync(int brandId);

        Task<List<Comment>> GetReviewFromProduct(int commentId);

        Task<int> CreateReviewAsync(int productId, int userId, Comment comment);

        Task<Comment> UpdateReviewAsync(int commentId);

        Task DeleteReviewAsync(int commentId);
    }
}
