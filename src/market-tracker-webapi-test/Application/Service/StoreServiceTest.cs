using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Operations.City;
using market_tracker_webapi.Application.Repository.Operations.Company;
using market_tracker_webapi.Application.Repository.Operations.Store;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Operations.Store;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class StoreServiceTest
{
    private readonly Mock<IStoreRepository> _storeRepositoryMock;
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;

    private readonly StoreService _storeService;

    public StoreServiceTest()
    {
        _storeRepositoryMock = new Mock<IStoreRepository>();
        _cityRepositoryMock = new Mock<ICityRepository>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();

        _storeService = new StoreService(
            _storeRepositoryMock.Object,
            _cityRepositoryMock.Object,
            _companyRepositoryMock.Object,
            new MockedTransactionManager()
        );
    }

    [Fact]
    public async Task GetStoresAsync_ShouldReturnStoresCollectionOutputModel()
    {
        // Arrange
        var stores = new List<Store>
        {
            new()
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            },
            new()
            {
                Id = 2,
                Name = "Store 2",
                Address = "Address 2",
                CityId = 2,
                CompanyId = 2
            }
        };

        _storeRepositoryMock.Setup(x => x.GetStoresAsync()).ReturnsAsync(stores);

        // Act
        var result = await _storeService.GetStoresAsync();

        // Assert
        result.Should().BeEquivalentTo(new CollectionOutputModel(stores));
    }

    [Fact]
    public async Task GetStoreByIdAsync_WhenStoreExists_ShouldReturnStore()
    {
        // Arrange
        var store = new Store
        {
            Id = 1,
            Name = "Store 1",
            Address = "Address 1",
            CityId = 1,
            CompanyId = 1
        };

        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(1)).ReturnsAsync(store);

        // Act
        var result = await _storeService.GetStoreByIdAsync(1);

        // Assert
        result.Value.Should().BeEquivalentTo(store);
    }

    [Fact]
    public async Task GetStoreByIdAsync_WhenStoreDoesNotExist_ReturnsStoreByIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Store?)null);

        // Act
        var result = await _storeService.GetStoreByIdAsync(It.IsAny<int>());

        // Assert
        result
            .Error.Should()
            .BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(It.IsAny<int>()));
    }

    [Fact]
    public async Task GetStoresFromCompanyAsync_WhenCompanyDoesNotExist_ShouldReturnStoreByCompanyIdNotFound()
    {
        // Arrange
        _companyRepositoryMock.Setup(x => x.GetCompanyByIdAsync(1)).ReturnsAsync((Company?)null);

        // Act
        var result = await _storeService.GetStoresFromCompanyAsync(1);

        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(1));
    }

    [Fact]
    public async Task GetStoresFromCompanyAsync_WhenCompanyExistsAndStoresExist_ShouldReturnStores()
    {
        // Arrange
        var stores = new List<Store>
        {
            new()
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            },
            new()
            {
                Id = 2,
                Name = "Store 2",
                Address = "Address 2",
                CityId = 2,
                CompanyId = 2
            }
        };

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync(
                new Company
                {
                    Id = 1,
                    Name = "Company 1",
                    CreatedAt = DateTime.Now
                }
            );

        _storeRepositoryMock.Setup(x => x.GetStoresFromCompanyAsync(1)).ReturnsAsync(stores);

        // Act
        var result = await _storeService.GetStoresFromCompanyAsync(1);

        // Assert
        result.Value.Should().BeEquivalentTo(stores);
    }

    [Fact]
    public async Task GetStoresByCityNameAsync_WhenCityDoesNotExist_ShouldReturnStoreByCityNameNotFound()
    {
        // Arrange
        _cityRepositoryMock.Setup(x => x.GetCityByNameAsync("City 1")).ReturnsAsync((City?)null);

        // Act
        var result = await _storeService.GetStoresByCityNameAsync("City 1");

        // Assert
        result
            .Error.Should()
            .BeEquivalentTo(new CityFetchingError.CityByNameNotFound("City 1"));
    }

    [Fact]
    public async Task GetStoresByCityNameAsync_WhenCityExistsAndStoresExist_ShouldReturnStores()
    {
        // Arrange
        var stores = new List<Store>
        {
            new()
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            },
            new()
            {
                Id = 2,
                Name = "Store 2",
                Address = "Address 2",
                CityId = 2,
                CompanyId = 2
            }
        };

        _cityRepositoryMock
            .Setup(x => x.GetCityByNameAsync("City 1"))
            .ReturnsAsync(new City { Id = 1, Name = "City 1" });

        _storeRepositoryMock.Setup(x => x.GetStoresByCityNameAsync("City 1")).ReturnsAsync(stores);

        // Act
        var result = await _storeService.GetStoresByCityNameAsync("City 1");

        // Assert
        result.Value.Should().BeEquivalentTo(stores);
    }

    [Fact]
    public async Task AddStoreAsync_WhenStoreAddressAlreadyExists_ShouldReturnStoreAddressAlreadyExists()
    {
        // Arrange
        var address = "Address 1";

        _storeRepositoryMock
            .Setup(x => x.GetStoreByAddressAsync(address))
            .ReturnsAsync(
                new Store
                {
                    Id = 1,
                    Name = "Store 1",
                    Address = "Address 1",
                    CityId = 1,
                    CompanyId = 1
                }
            );

        // Act
        var result = await _storeService.AddStoreAsync("Store 1", "Address 1", 1, 1);

        // Assert
        result
            .Error.Should()
            .BeEquivalentTo(new StoreCreationError.StoreAddressAlreadyExists(address));
    }

    [Fact]
    public async Task AddStoreAsync_WhenCityDoesNotExist_ShouldReturnStoreByCityIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByAddressAsync(It.IsAny<string>()))
            .ReturnsAsync((Store?)null);

        _cityRepositoryMock.Setup(x => x.GetCityByIdAsync(1)).ReturnsAsync((City?)null);

        // Act
        var result = await _storeService.AddStoreAsync("Store 1", "Address 1", 1, 1);

        // Assert
        result.Error.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(1));
    }

    [Fact]
    public async Task AddStoreAsync_WhenCompanyDoesNotExist_ShouldReturnStoreByCompanyIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByAddressAsync(It.IsAny<string>()))
            .ReturnsAsync((Store?)null);

        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync(new City { Id = 1, Name = "City 1" });

        _companyRepositoryMock.Setup(x => x.GetCompanyByIdAsync(1)).ReturnsAsync((Company?)null);

        // Act
        var result = await _storeService.AddStoreAsync("Store 1", "Address 1", 1, 1);

        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(1));
    }

    [Fact]
    public async Task AddStoreAsync_WhenStoreIsAdded_ShouldReturnIdOutputModel()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByAddressAsync(It.IsAny<string>()))
            .ReturnsAsync((Store?)null);

        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync(new City { Id = 1, Name = "City 1" });

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync(
                new Company
                {
                    Id = 1,
                    Name = "Company 1",
                    CreatedAt = DateTime.Now
                }
            );

        _storeRepositoryMock
            .Setup(x => x.AddStoreAsync("Store 1", "Address 1", 1, 1))
            .ReturnsAsync(1);

        // Act
        var result = await _storeService.AddStoreAsync("Store 1", "Address 1", 1, 1);

        // Assert
        result.Value.Should().BeEquivalentTo(new IntIdOutputModel(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WhenStoreDoesNotExist_ShouldReturnStoreByIdNotFound()
    {
        // Arrange
        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(1)).ReturnsAsync((Store?)null);

        // Act
        var result = await _storeService.UpdateStoreAsync(1, "Store1", "Address 1", 1, 1);

        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WhenCityDoesNotExist_ShouldReturnStoreByCityIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync(
                new Store
                {
                    Id = 1,
                    Name = "Store 1",
                    Address = "Address 1",
                    CityId = 1,
                    CompanyId = 1
                }
            );

        _cityRepositoryMock.Setup(x => x.GetCityByIdAsync(1)).ReturnsAsync((City?)null);

        // Act
        var result = await _storeService.UpdateStoreAsync(1, "Store1", "Address 1", 1, 1);

        // Assert
        result.Error.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WhenCompanyDoesNotExist_ShouldReturnStoreByCompanyIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync(
                new Store
                {
                    Id = 1,
                    Name = "Store 1",
                    Address = "Address 1",
                    CityId = 1,
                    CompanyId = 1
                }
            );

        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync(new City { Id = 1, Name = "City 1" });

        _companyRepositoryMock.Setup(x => x.GetCompanyByIdAsync(1)).ReturnsAsync((Company?)null);

        // Act
        var result = await _storeService.UpdateStoreAsync(1, "Store 1", "Address 1", 1, 1);

        // Assert
        result.Error.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WhenStoreIsUpdated_ShouldReturnIdOutputModel()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync(
                new Store
                {
                    Id = 1,
                    Name = "Store 1",
                    Address = "Address 1",
                    CityId = 1,
                    CompanyId = 1
                }
            );

        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync(new City { Id = 1, Name = "City 1" });

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync(
                new Company
                {
                    Id = 1,
                    Name = "Company 1",
                    CreatedAt = DateTime.Now
                }
            );

        _storeRepositoryMock
            .Setup(x => x.UpdateStoreAsync(1, "Address 1", 1, 1))
            .ReturnsAsync(
                new Store
                {
                    Id = 1,
                    Name = "Store 1",
                    Address = "Address 1",
                    CityId = 1,
                    CompanyId = 1
                }
            );

        // Act
        var result = await _storeService.UpdateStoreAsync(1, "Store 1", "Address 1", 1, 1);

        // Assert
        result.Value.Should().BeEquivalentTo(new IntIdOutputModel(1));
    }

    [Fact]
    public async Task DeleteStoreAsync_WhenStoreDoesNotExist_ShouldReturnStoreByIdNotFound()
    {
        // Arrange
        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(1)).ReturnsAsync((Store?)null);

        // Act
        var result = await _storeService.DeleteStoreAsync(1);

        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(1));
    }

    [Fact]
    public async Task DeleteStoreAsync_WhenStoreIsDeleted_ShouldReturnIdOutputModel()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync(
                new Store
                {
                    Id = 1,
                    Name = "Store 1",
                    Address = "Address 1",
                    CityId = 1,
                    CompanyId = 1
                }
            );

        _storeRepositoryMock
            .Setup(x => x.DeleteStoreAsync(1))
            .ReturnsAsync(
                new Store
                {
                    Id = 1,
                    Name = "Store 1",
                    Address = "Address 1",
                    CityId = 1,
                    CompanyId = 1
                }
            );

        // Act
        var result = await _storeService.DeleteStoreAsync(1);

        // Assert
        result.Value.Should().BeEquivalentTo(new IntIdOutputModel(1));
    }
}
