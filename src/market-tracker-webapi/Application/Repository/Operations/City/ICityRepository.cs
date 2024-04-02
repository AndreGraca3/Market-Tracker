using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Application.Repository.Operations.City;

public interface ICityRepository
{
    Task<IEnumerable<Domain.City>> GetCitiesAsync();
    Task<Domain.City?> GetCityByIdAsync(int id);

    Task<Domain.City?> GetCityByNameAsync(string name);

    Task<int> AddCityAsync(string name);

    Task<Domain.City?> UpdateCityAsync(int id, string name);

    Task<Domain.City?> DeleteCityAsync(int id);
}
