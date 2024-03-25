using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;

namespace market_tracker_webapi.Application.Repository.Interfaces
{
    public interface IBrandRepository
    {
        Task<Brand?> GetBrandByIdAsync(int brandId);

        Task<Brand?> GetBrandByNameAsync(string brandName);

        Task<Brand> AddBrandAsync(string name);

        Task<Brand?> RemoveBrandAsync(int brandId);
    }
}
