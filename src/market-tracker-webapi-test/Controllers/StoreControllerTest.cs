using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Controllers;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Operations.Store;
using market_tracker_webapi.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Controllers;

public class StoreControllerTest
{
    private readonly Mock<IStoreService> _storeServiceMock;
    private readonly StoreController _storeController;

    public StoreControllerTest()
    {
        _storeServiceMock = new Mock<IStoreService>();
        _storeController = new StoreController(_storeServiceMock.Object);
    }

    [Fact]
    public async Task GetStoresAsync_ReturnsOk()
    {
        // Arrange
        var stores = new List<Store>
        {
            new Store
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            },
            new Store
            {
                Id = 2,
                Name = "Store 2",
                Address = "Address 2",
                CityId = 2,
                CompanyId = 2
            }
        };
        _storeServiceMock
            .Setup(service => service.GetStoresAsync())
            .ReturnsAsync(new CollectionOutputModel(stores));

        // Act
        var result = await _storeController.GetStoresAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualStoresCollection = Assert.IsType<CollectionOutputModel>(okResult.Value);
        actualStoresCollection.Should().BeEquivalentTo(new CollectionOutputModel(stores));
        actualStoresCollection.Results.Should().BeEquivalentTo(stores);
    }

    [Fact]
    public async Task GetStoreByIdAsync_RespondsWith_Ok_ReturnsObjectAsync()
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
        _storeServiceMock
            .Setup(service => service.GetStoreByIdAsync(1))
            .ReturnsAsync(EitherExtensions.Success<StoreFetchingError, Store>(store));

        // Act
        var result = await _storeController.GetStoreByIdAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualStore = Assert.IsType<Store>(okResult.Value);
        actualStore.Should().BeEquivalentTo(store);
    }

    [Fact]
    public async Task GetStoreByIdAsync_ReturnsNotFound()
    {
        // Arrange
        _storeServiceMock
            .Setup(service => service.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(
                EitherExtensions.Failure<StoreFetchingError, Store>(
                    new StoreFetchingError.StoreByIdNotFound(It.IsAny<int>())
                )
            );

        // Act
        var actual = await _storeController.GetStoreByIdAsync(It.IsAny<int>());

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreByIdNotFound>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(It.IsAny<int>()));
    }

    [Fact]
    public async Task GetStoresFromCompanyAsync_ReturnsOk()
    {
        // Arrange
        var stores = new List<Store>
        {
            new Store
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            },
            new Store
            {
                Id = 2,
                Name = "Store 2",
                Address = "Address 2",
                CityId = 2,
                CompanyId = 2
            }
        };
        _storeServiceMock
            .Setup(service => service.GetStoresFromCompanyAsync(1))
            .ReturnsAsync(EitherExtensions.Success<StoreFetchingError, IEnumerable<Store>>(stores));

        // Act
        var result = await _storeController.GetStoresFromCompanyAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualStores = Assert.IsType<List<Store>>(okResult.Value);
        actualStores.Should().BeEquivalentTo(stores);
    }

    [Fact]
    public async Task GetStoresFromCompanyAsync_ReturnsNotFound()
    {
        // Arrange
        _storeServiceMock
            .Setup(service => service.GetStoresFromCompanyAsync(It.IsAny<int>()))
            .ReturnsAsync(
                EitherExtensions.Failure<StoreFetchingError, IEnumerable<Store>>(
                    new StoreFetchingError.StoreByCompanyIdNotFound(It.IsAny<int>())
                )
            );

        // Act
        var actual = await _storeController.GetStoresFromCompanyAsync(It.IsAny<int>());

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreByCompanyIdNotFound>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new StoreFetchingError.StoreByCompanyIdNotFound(It.IsAny<int>()));
    }

    [Fact]
    public async Task GetStoresByCityNameAsync_ReturnsOk()
    {
        // Arrange
        var stores = new List<Store>
        {
            new Store
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CityId = 1,
                CompanyId = 1
            },
            new Store
            {
                Id = 2,
                Name = "Store 2",
                Address = "Address 2",
                CityId = 2,
                CompanyId = 2
            }
        };
        _storeServiceMock
            .Setup(service => service.GetStoresByCityNameAsync("City"))
            .ReturnsAsync(EitherExtensions.Success<StoreFetchingError, IEnumerable<Store>>(stores));

        // Act
        var result = await _storeController.GetStoresByCityNameAsync("City");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualStores = Assert.IsType<List<Store>>(okResult.Value);
        actualStores.Should().BeEquivalentTo(stores);
    }

    [Fact]
    public async Task GetStoresByCityNameAsync_ReturnsNotFound()
    {
        // Arrange
        _storeServiceMock
            .Setup(service => service.GetStoresByCityNameAsync(It.IsAny<string>()))
            .ReturnsAsync(
                EitherExtensions.Failure<StoreFetchingError, IEnumerable<Store>>(
                    new StoreFetchingError.StoreByCityNameNotFound(It.IsAny<string>())
                )
            );

        // Act
        var actual = await _storeController.GetStoresByCityNameAsync(It.IsAny<string>());

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreByCityNameNotFound>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new StoreFetchingError.StoreByCityNameNotFound(It.IsAny<string>()));
    }

    [Fact]
    public async Task AddStoreAsync_ReturnsOk()
    {
        // Arrange
        var idOutputModel = new IdOutputModel(1);
        _storeServiceMock
            .Setup(service => service.AddStoreAsync("Store", "Address", 1, 1))
            .ReturnsAsync(EitherExtensions.Success<IStoreError, IdOutputModel>(idOutputModel));

        // Act
        var result = await _storeController.AddStoreAsync(
            new AddStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualIdOutputModel = Assert.IsType<IdOutputModel>(okResult.Value);
        actualIdOutputModel.Should().BeEquivalentTo(idOutputModel);
    }

    [Fact]
    public async Task AddStoreAsync_WithInvalidCityId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.AddStoreAsync("Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IStoreError, IdOutputModel>(
                    new StoreFetchingError.StoreByCityIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.AddStoreAsync(
            new AddStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreByCityIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new StoreFetchingError.StoreByCityIdNotFound(1));
    }

    [Fact]
    public async Task AddStoreAsync_WithInvalidStoreName_ReturnsConflict()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.AddStoreAsync("Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IStoreError, IdOutputModel>(
                    new StoreCreationError.StoreNameAlreadyExists("Store")
                )
            );

        //Act
        var actual = await _storeController.AddStoreAsync(
            new AddStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreNameAlreadyExists>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new StoreCreationError.StoreNameAlreadyExists("Store"));
    }

    [Fact]
    public async Task AddStoreAsync_WithInvalidCompanyId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.AddStoreAsync("Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IStoreError, IdOutputModel>(
                    new StoreFetchingError.StoreByCompanyIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.AddStoreAsync(
            new AddStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreByCompanyIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new StoreFetchingError.StoreByCompanyIdNotFound(1));
    }

    [Fact]
    public async Task AddStoreAsync_WithInvalidAddress_ReturnsConflict()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.AddStoreAsync("Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IStoreError, IdOutputModel>(
                    new StoreCreationError.StoreAddressAlreadyExists("Address")
                )
            );

        //Act
        var actual = await _storeController.AddStoreAsync(
            new AddStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreAddressAlreadyExists>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new StoreCreationError.StoreAddressAlreadyExists("Address"));
    }

    [Fact]
    public async Task UpdateStoreAsync_ReturnsOk()
    {
        // Arrange
        var idOutputModel = new IdOutputModel(1);
        _storeServiceMock
            .Setup(service => service.UpdateStoreAsync(1, "Store", "Address", 1, 1))
            .ReturnsAsync(EitherExtensions.Success<IStoreError, IdOutputModel>(idOutputModel));

        // Act
        var result = await _storeController.UpdateStoreAsync(
            1,
            new UpdateStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualIdOutputModel = Assert.IsType<IdOutputModel>(okResult.Value);
        actualIdOutputModel.Should().BeEquivalentTo(idOutputModel);
    }

    [Fact]
    public async Task UpdateStoreAsync_WithInvalidCityId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.UpdateStoreAsync(1, "Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IStoreError, IdOutputModel>(
                    new StoreFetchingError.StoreByCityIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.UpdateStoreAsync(
            1,
            new UpdateStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreByCityIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new StoreFetchingError.StoreByCityIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WithInvalidStoreName_ReturnsConflict()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.UpdateStoreAsync(1, "Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IStoreError, IdOutputModel>(
                    new StoreCreationError.StoreNameAlreadyExists("Store")
                )
            );

        //Act
        var actual = await _storeController.UpdateStoreAsync(
            1,
            new UpdateStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreNameAlreadyExists>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new StoreCreationError.StoreNameAlreadyExists("Store"));
    }

    [Fact]
    public async Task UpdateStoreAsync_WithInvalidCompanyId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.UpdateStoreAsync(1, "Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IStoreError, IdOutputModel>(
                    new StoreFetchingError.StoreByCompanyIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.UpdateStoreAsync(
            1,
            new UpdateStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreByCompanyIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new StoreFetchingError.StoreByCompanyIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WithInvalidStoreId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.UpdateStoreAsync(1, "Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IStoreError, IdOutputModel>(
                    new StoreFetchingError.StoreByIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.UpdateStoreAsync(
            1,
            new UpdateStoreInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreByIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(1));
    }

    [Fact]
    public async Task DeleteStoreAsync_ReturnsOk()
    {
        // Arrange
        var idOutputModel = new IdOutputModel(1);
        _storeServiceMock
            .Setup(service => service.DeleteStoreAsync(1))
            .ReturnsAsync(
                EitherExtensions.Success<StoreFetchingError, IdOutputModel>(idOutputModel)
            );

        // Act
        var result = await _storeController.DeleteStoreAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualIdOutputModel = Assert.IsType<IdOutputModel>(okResult.Value);
        actualIdOutputModel.Should().BeEquivalentTo(idOutputModel);
    }

    [Fact]
    public async Task DeleteStoreAsync_WithInvalidStoreId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.DeleteStoreAsync(1))
            .ReturnsAsync(
                EitherExtensions.Failure<StoreFetchingError, IdOutputModel>(
                    new StoreFetchingError.StoreByIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.DeleteStoreAsync(1);

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<StoreProblem.StoreByIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(1));
    }
}
