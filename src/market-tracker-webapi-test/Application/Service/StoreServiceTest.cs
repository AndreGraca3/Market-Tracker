using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Operations.Market.Store;
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
        _storeRepositoryMock
            .Setup(x => x.GetStoresAsync(null, null, null))
            .ReturnsAsync(MockedData.DummyStores);

        // Act
        var result = await _storeService.GetStoresAsync();

        // Assert
        result.Should().BeEquivalentTo(MockedData.DummyStores);
    }

    [Fact]
    public async Task GetStoreByIdAsync_WhenStoreExists_ShouldReturnStore()
    {
        // Arrange
        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(1)).ReturnsAsync(MockedData.DummyStores[0]);

        // Act
        var result = await _storeService.GetStoreByIdAsync(1);

        // Assert
        result.Should().BeEquivalentTo(MockedData.DummyStores[0]);
    }

    [Fact]
    public async Task GetStoreByIdAsync_WhenStoreDoesNotExist_ReturnsStoreByIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Store?)null);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _storeService.GetStoreByIdAsync(It.IsAny<int>()));

        // Assert
        result
            .ServiceError
            .Should()
            .BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(It.IsAny<int>()));
    }

    [Fact]
    public async Task GetStoresFromCompanyAsync_WhenCompanyDoesNotExist_ShouldReturnStoreByCompanyIdNotFound()
    {
        // Arrange
        _companyRepositoryMock.Setup(x => x.GetCompaniesAsync()).ReturnsAsync(new List<Company>());

        // Act
        var result = await _storeService.GetStoresAsync(1);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetStoresFromCompanyAsync_WhenCompanyExistsAndStoresExist_ShouldReturnStores()
    {
        // Arrange
        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync(MockedData.DummyStores[0].Company);

        _storeRepositoryMock
            .Setup(x => x.GetStoresAsync(1, null, null))
            .ReturnsAsync(MockedData.DummyStores);

        // Act
        var result = await _storeService.GetStoresAsync(1);

        // Assert
        result.Should().BeEquivalentTo(MockedData.DummyStores);
    }

    [Fact]
    public async Task UpdateStoreAsync_WhenStoreDoesNotExist_ShouldReturnStoreByIdNotFound()
    {
        // Arrange
        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(1)).ReturnsAsync((Store?)null);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _storeService.UpdateStoreAsync(1, "Store1", "Address 1", 1, 1));

        // Assert
        result.ServiceError.Should().BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WhenCityDoesNotExist_ShouldReturnStoreByCityIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(MockedData.DummyStores[0].Id.Value))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _cityRepositoryMock.Setup(x => x.GetCityByIdAsync(1)).ReturnsAsync((City?)null);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _storeService.UpdateStoreAsync(1, "Store1", "Address 1", 1, 1));

        // Assert
        result.ServiceError.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WhenCompanyDoesNotExist_ShouldReturnStoreByCompanyIdNotFound()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(MockedData.DummyStores[0].Id.Value))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(MockedData.DummyStores[0].City!.Id.Value))
            .ReturnsAsync(MockedData.DummyStores[0].City);

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(1))
            .ReturnsAsync((Company?)null);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _storeService.UpdateStoreAsync(1, "Store 1", "Address 1", 1, 1));

        // Assert
        result.ServiceError.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WhenStoreIsUpdated_ShouldReturnIdOutputModel()
    {
        // Arrange
        var newStore = new Store(1, "new Store 1", "Address 1", new City(1, "city1"),
            new Company(1, "company1", "company1", DateTime.UtcNow), Guid.NewGuid());

        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(MockedData.DummyStores[0].Id.Value))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _cityRepositoryMock
            .Setup(x => x.GetCityByIdAsync(MockedData.DummyStores[0].City!.Id.Value))
            .ReturnsAsync(MockedData.DummyStores[0].City);

        _companyRepositoryMock
            .Setup(x => x.GetCompanyByIdAsync(MockedData.DummyStores[0].Company.Id.Value))
            .ReturnsAsync(MockedData.DummyStores[0].Company);

        _storeRepositoryMock
            .Setup(x => x.UpdateStoreAsync(1, "Address 1", 1, 1))
            .ReturnsAsync(new StoreItem(newStore.Id, newStore.Name, newStore.Address, newStore.City?.Id.Value,
                newStore.Company.Id.Value, newStore.OperatorId));

        // Act
        var result = await _storeService.UpdateStoreAsync(1, "Store 1", "Address 1", 1, 1);

        // Assert
        result.Should().BeEquivalentTo(new StoreItem(newStore.Id, newStore.Name, newStore.Address,
            newStore.City?.Id.Value,
            newStore.Company.Id.Value, newStore.OperatorId));
    }

    [Fact]
    public async Task DeleteStoreAsync_WhenStoreDoesNotExist_ShouldReturnStoreByIdNotFound()
    {
        // Arrange
        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(1)).ReturnsAsync((Store?)null);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _storeService.DeleteStoreAsync(1));

        // Assert
        ex.ServiceError
            .Should()
            .BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(1));
    }

    [Fact]
    public async Task DeleteStoreAsync_WhenStoreIsDeleted_ShouldReturnId()
    {
        // Arrange
        _storeRepositoryMock
            .Setup(x => x.GetStoreByIdAsync(MockedData.DummyStores[0].Id.Value))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _storeRepositoryMock
            .Setup(x => x.DeleteStoreAsync(MockedData.DummyStores[0].Id.Value))
            .ReturnsAsync(new StoreItem(MockedData.DummyStores[0].Id, MockedData.DummyStores[0].Name, MockedData.DummyStores[0].Address,
                MockedData.DummyStores[0].City?.Id.Value, MockedData.DummyStores[0].Company.Id.Value, MockedData.DummyStores[0].OperatorId));

        // Act
        var result = await _storeService.DeleteStoreAsync(MockedData.DummyStores[0].Id.Value);

        // Assert
        result.Should().BeEquivalentTo(new StoreId(MockedData.DummyStores[0].Id.Value));
    }
}