using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repositories.City;
using market_tracker_webapi.Application.Repositories.Company;
using market_tracker_webapi.Application.Repositories.Store;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Operations.Store;
using market_tracker_webapi.Application.Services.Transaction;
using market_tracker_webapi.Application.Utils;
using Moq;

namespace market_tracker_webapi_test.Application.Services;

public class StoreServiceTest
{
    private readonly Mock<IStoreRepository> _storeRepositoryMock;
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<ITransactionManager> _transactionManagerMock;
    
    private readonly StoreService _storeService;
    
    public StoreServiceTest()
    {
        _storeRepositoryMock = new Mock<IStoreRepository>();
        _cityRepositoryMock = new Mock<ICityRepository>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _transactionManagerMock = new Mock<ITransactionManager>();
        
        _storeService = new StoreService(
            _storeRepositoryMock.Object,
            _cityRepositoryMock.Object,
            _companyRepositoryMock.Object,
            _transactionManagerMock.Object
        );
    }
    
    [Fact]
    public async Task GetStoresAsync_ShouldReturnStores()
    {
        // Arrange
        var stores = new List<StoreDomain>
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
        
        _storeRepositoryMock
            .Setup(x => x.GetStoresAsync())
            .ReturnsAsync(stores);
        
        // Act
        var result = await _storeService.GetStoresAsync();
        
        // Assert
        result.Should().BeEquivalentTo(stores);
    }
    
    [Fact]
    public async Task GetStoreByIdAsync_WhenStoreExists_ShouldReturnStore()
    {
        // Arrange
        var store = new StoreDomain
        {
            Id = 1,
            Name = "Store 1",
            Address = "Address 1",
            CityId = 1,
            CompanyId = 1
        };
        
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync(store);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<StoreDomain?>>>()))
            .ReturnsAsync(store);
        
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
            .ReturnsAsync((StoreDomain?)null);
        
        // Act
        var result = await _storeService.GetStoreByIdAsync(It.IsAny<int>());
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(It.IsAny<int>()));
    }
    
    [Fact]
    public async Task AddStoreAsync_WhenStoreAddressAlreadyExists_ShouldReturnStoreAddressAlreadyExists()
    {
        // Arrange
        var address = "Address 1";
        
        _storeRepositoryMock
            .Setup(x => x.GetStoreByAddressAsync(address))
            .ReturnsAsync(new StoreDomain
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            });
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<IStoreError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<IStoreError, IdOutputModel>(
                new StoreCreationError.StoreAddressAlreadyExists(address)
            ));
        
        // Act
        var result = await _storeService.AddStoreAsync("Store 1", "Address 1", 1, 1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreCreationError.StoreAddressAlreadyExists(address));
    }
    
    [Fact]
    public async Task AddStoreAsync_WhenCityDoesNotExist_ShouldReturnStoreByCityIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByAddressAsync(It.IsAny<string>()))
            .ReturnsAsync((StoreDomain?)null);
        
        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync((CityDomain?)null);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<IStoreError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<IStoreError, IdOutputModel>(
                new StoreFetchingError.StoreByCityIdNotFound(1)
            ));
        
        // Act
        var result = await _storeService.AddStoreAsync("Store 1", "Address 1", 1, 1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByCityIdNotFound(1));
    }
    
    [Fact]
    public async Task AddStoreAsync_WhenCompanyDoesNotExist_ShouldReturnStoreByCompanyIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByAddressAsync(It.IsAny<string>()))
            .ReturnsAsync((StoreDomain?)null);
        
        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync(new CityDomain
            {
                Id = 1,
                Name = "City 1"
            });
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync((CompanyDomain?)null);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<IStoreError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<IStoreError, IdOutputModel>(
                new StoreFetchingError.StoreByCompanyIdNotFound(1)
            ));
        
        // Act
        var result = await _storeService.AddStoreAsync("Store 1", "Address 1", 1, 1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByCompanyIdNotFound(1));
    }
    
    [Fact]
    public async Task AddStoreAsync_WhenStoreIsAdded_ShouldReturnIdOutputModel()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByAddressAsync(It.IsAny<string>()))
            .ReturnsAsync((StoreDomain?)null);
        
        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync(new CityDomain
            {
                Id = 1,
                Name = "City 1"
            });
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync(new CompanyDomain
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = DateTime.Now
            });
        
        _storeRepositoryMock
            .Setup(x => x.AddStoreAsync("Store 1", "Address 1", 1, 1))
            .ReturnsAsync(1);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<IStoreError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Success<IStoreError, IdOutputModel>(
                new IdOutputModel
                {
                    Id = 1
                }
            ));
        
        // Act
        var result = await _storeService.AddStoreAsync("Store 1", "Address 1", 1, 1);
        
        // Assert
        result.Value.Should().BeEquivalentTo(new IdOutputModel
        {
            Id = 1
        });
    }
    
    [Fact]
    public async Task UpdateStoreAsync_WhenStoreDoesNotExist_ShouldReturnStoreByIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync((StoreDomain?)null);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<StoreFetchingError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
                new StoreFetchingError.StoreByIdNotFound(1)
            ));
        
        // Act
        var result = await _storeService.UpdateStoreAsync(1, "Address 1", 1, 1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(1));
    }
    
    [Fact]
    public async Task UpdateStoreAsync_WhenCityDoesNotExist_ShouldReturnStoreByCityIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync(new StoreDomain
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            });
        
        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync((CityDomain?)null);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<StoreFetchingError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
                new StoreFetchingError.StoreByCityIdNotFound(1)
            ));
        
        // Act
        var result = await _storeService.UpdateStoreAsync(1, "Address 1", 1, 1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByCityIdNotFound(1));
    }
    
    [Fact]
    public async Task UpdateStoreAsync_WhenCompanyDoesNotExist_ShouldReturnStoreByCompanyIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync(new StoreDomain
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            });
        
        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync(new CityDomain
            {
                Id = 1,
                Name = "City 1"
            });
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync((CompanyDomain?)null);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<StoreFetchingError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
                new StoreFetchingError.StoreByCompanyIdNotFound(1)
            ));
        
        // Act
        var result = await _storeService.UpdateStoreAsync(1, "Address 1", 1, 1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByCompanyIdNotFound(1));
    }
    
    [Fact]
    public async Task UpdateStoreAsync_WhenStoreIsUpdated_ShouldReturnIdOutputModel()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync(new StoreDomain
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            });
        
        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(1))
            .ReturnsAsync(new CityDomain
            {
                Id = 1,
                Name = "City 1"
            });
        
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync(new CompanyDomain
            {
                Id = 1,
                Name = "Company 1",
                CreatedAt = DateTime.Now
            });
        
        _storeRepositoryMock
            .Setup(x => x.UpdateStoreAsync(1, "Address 1", 1, 1))
            .ReturnsAsync(new StoreDomain
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            });
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<StoreFetchingError, IdOutputModel>>>>()))
            .ReturnsAsync(EitherExtensions.Success<StoreFetchingError, IdOutputModel>(
                new IdOutputModel
                {
                    Id = 1
                }
            ));
        
        // Act
        var result = await _storeService.UpdateStoreAsync(1, "Address 1", 1, 1);
        
        // Assert
        result.Value.Should().BeEquivalentTo(new IdOutputModel
        {
            Id = 1
        });
    }
    
    [Fact]
    public async Task DeleteStoreAsync_WhenStoreDoesNotExist_ShouldReturnStoreByIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(1))
            .ReturnsAsync((StoreDomain?)null);
        
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
            .ReturnsAsync(new StoreDomain
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            });
        
        _storeRepositoryMock
            .Setup(x => x.DeleteStoreAsync(1))
            .ReturnsAsync(new StoreDomain
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            });
        
        // Act
        var result = await _storeService.DeleteStoreAsync(1);
        
        // Assert
        result.Value.Should().BeEquivalentTo(new IdOutputModel
        {
            Id = 1
        });
    }
    
    [Fact]
    public async Task GetStoresFromCompany_WhenCompanyDoesNotExist_ShouldReturnStoreByCompanyIdNotFound()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync((CompanyDomain?)null);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<StoreFetchingError, IEnumerable<StoreDomain>>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<StoreFetchingError, IEnumerable<StoreDomain>>(
                new StoreFetchingError.StoreByCompanyIdNotFound(1)));
        
        // Act
        var result = await _storeService.GetStoresFromCompany(1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByCompanyIdNotFound(1));
    }
    
    [Fact]
    public async Task GetStoresFromCityByName_WhenCityDoesNotExist_ShouldReturnStoreByCityIdNotFound()
    {
        // Arrange
        _cityRepositoryMock
            .Setup(x => x.GetCityByNameAsync("City 1"))
            .ReturnsAsync((CityDomain?)null);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<StoreFetchingError, IEnumerable<StoreDomain>>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<StoreFetchingError, IEnumerable<StoreDomain>>(
                new StoreFetchingError.StoreByCityIdNotFound(1)));
        
        // Act
        var result = await _storeService.GetStoresFromCityByName("City 1");
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByCityIdNotFound(1));
    }
    
    [Fact]
    public async Task GetStoresFromCityByName_WhenCityExistsButNoStores_ShouldReturnStoreByCityNameNotFound()
    {
        // Arrange
        _cityRepositoryMock
            .Setup(x => x.GetCityByNameAsync("City 1"))
            .ReturnsAsync(new CityDomain
            {
                Id = 1,
                Name = "City 1"
            });
        
        _storeRepositoryMock
            .Setup(x => x.GetStoresByCityNameAsync("City 1"))
            .ReturnsAsync(new List<StoreDomain>());
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<StoreFetchingError, IEnumerable<StoreDomain>>>>>()))
            .ReturnsAsync(EitherExtensions.Failure<StoreFetchingError, IEnumerable<StoreDomain>>(
                new StoreFetchingError.StoreByCityNameNotFound("City 1")));
        
        // Act
        var result = await _storeService.GetStoresFromCityByName("City 1");
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByCityNameNotFound("City 1"));
    }
    
    [Fact]
    public async Task GetStoresFromCityByName_WhenCityExistsAndStoresExist_ShouldReturnStores()
    {
        // Arrange
        var stores = new List<StoreDomain>
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
            .ReturnsAsync(new CityDomain
            {
                Id = 1,
                Name = "City 1"
            });
        
        _storeRepositoryMock
            .Setup(x => x.GetStoresByCityNameAsync("City 1"))
            .ReturnsAsync(stores);
        
        _transactionManagerMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<Task<Either<StoreFetchingError, IEnumerable<StoreDomain>>>>>()))
            .ReturnsAsync(EitherExtensions.Success<StoreFetchingError, IEnumerable<StoreDomain>>(stores));
        
        // Act
        var result = await _storeService.GetStoresFromCityByName("City 1");
        
        // Assert
        result.Value.Should().BeEquivalentTo(stores);
    }
    
}