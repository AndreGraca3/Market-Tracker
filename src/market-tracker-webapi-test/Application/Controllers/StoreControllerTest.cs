namespace market_tracker_webapi_test.Application.Controllers;

/*
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
            .ReturnsAsync(EitherExtensions.Success<IServiceError, CollectionOutputModel>(new CollectionOutputModel(stores)));

        // Act
        var result = await _storeController.GetStoresAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualStoresCollection = Assert.IsType<CollectionOutputModel>(okResult.Value);
        actualStoresCollection.Should().BeEquivalentTo(new CollectionOutputModel(stores));
        actualStoresCollection.Items.Should().BeEquivalentTo(stores);
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
            .ReturnsAsync(EitherExtensions.Success<CompanyFetchingError, IEnumerable<Store>>(stores));

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
                EitherExtensions.Failure<CompanyFetchingError, IEnumerable<Store>>(
                    new CompanyFetchingError.CompanyByIdNotFound(It.IsAny<int>())
                )
            );

        // Act
        var actual = await _storeController.GetStoresFromCompanyAsync(It.IsAny<int>());

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<CompanyProblem.CompanyByIdNotFound>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(It.IsAny<int>()));
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
            .ReturnsAsync(EitherExtensions.Success<CityFetchingError, IEnumerable<Store>>(stores));

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
                EitherExtensions.Failure<CityFetchingError, IEnumerable<Store>>(
                    new CityFetchingError.CityByNameNotFound(It.IsAny<string>())
                )
            );

        // Act
        var actual = await _storeController.GetStoresByCityNameAsync(It.IsAny<string>());

        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<CityProblem.CityNameNotFound>(result.Value);
        problem
            .Data.Should()
            .BeEquivalentTo(new CityFetchingError.CityByNameNotFound(It.IsAny<string>()));
    }

    [Fact]
    public async Task AddStoreAsync_ReturnsOk()
    {
        // Arrange
        var idOutputModel = new IntIdOutputModel(1);
        _storeServiceMock
            .Setup(service => service.AddStoreAsync("Store", "Address", 1, 1))
            .ReturnsAsync(EitherExtensions.Success<IServiceError, IntIdOutputModel>(idOutputModel));

        // Act
        var result = await _storeController.AddStoreAsync(
            new StoreCreationInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualIdOutputModel = Assert.IsType<IntIdOutputModel>(okResult.Value);
        actualIdOutputModel.Should().BeEquivalentTo(idOutputModel);
    }

    [Fact]
    public async Task AddStoreAsync_WithInvalidCityId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.AddStoreAsync("Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new CityFetchingError.CityByIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.AddStoreAsync(
            new StoreCreationInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<CityProblem.CityByIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(1));
    }

    [Fact]
    public async Task AddStoreAsync_WithInvalidStoreName_ReturnsConflict()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.AddStoreAsync("Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new StoreCreationError.StoreNameAlreadyExists("Store")
                )
            );

        //Act
        var actual = await _storeController.AddStoreAsync(
            new StoreCreationInputModel
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
                EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new CompanyFetchingError.CompanyByIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.AddStoreAsync(
            new StoreCreationInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<CompanyProblem.CompanyByIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(1));
    }

    [Fact]
    public async Task AddStoreAsync_WithInvalidAddress_ReturnsConflict()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.AddStoreAsync("Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new StoreCreationError.StoreAddressAlreadyExists("Address")
                )
            );

        //Act
        var actual = await _storeController.AddStoreAsync(
            new StoreCreationInputModel
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
        var idOutputModel = new IntIdOutputModel(1);
        _storeServiceMock
            .Setup(service => service.UpdateStoreAsync(1, "Store", "Address", 1, 1))
            .ReturnsAsync(EitherExtensions.Success<IServiceError, IntIdOutputModel>(idOutputModel));

        // Act
        var result = await _storeController.UpdateStoreAsync(
            1,
            new StoreUpdateInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualIdOutputModel = Assert.IsType<IntIdOutputModel>(okResult.Value);
        actualIdOutputModel.Should().BeEquivalentTo(idOutputModel);
    }

    [Fact]
    public async Task UpdateStoreAsync_WithInvalidCityId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.UpdateStoreAsync(1, "Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new CityFetchingError.CityByIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.UpdateStoreAsync(
            1,
            new StoreUpdateInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<CityProblem.CityByIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WithInvalidStoreName_ReturnsConflict()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.UpdateStoreAsync(1, "Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new StoreCreationError.StoreNameAlreadyExists("Store")
                )
            );

        //Act
        var actual = await _storeController.UpdateStoreAsync(
            1,
            new StoreUpdateInputModel
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
                EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new CompanyFetchingError.CompanyByIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.UpdateStoreAsync(
            1,
            new StoreUpdateInputModel
            {
                Name = "Store",
                Address = "Address",
                CityId = 1,
                CompanyId = 1
            }
        );

        //Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsAssignableFrom<CompanyProblem.CompanyByIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new CompanyFetchingError.CompanyByIdNotFound(1));
    }

    [Fact]
    public async Task UpdateStoreAsync_WithInvalidStoreId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.UpdateStoreAsync(1, "Store", "Address", 1, 1))
            .ReturnsAsync(
                EitherExtensions.Failure<IServiceError, IntIdOutputModel>(
                    new StoreFetchingError.StoreByIdNotFound(1)
                )
            );

        //Act
        var actual = await _storeController.UpdateStoreAsync(
            1,
            new StoreUpdateInputModel
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
        var idOutputModel = new IntIdOutputModel(1);
        _storeServiceMock
            .Setup(service => service.DeleteStoreAsync(1))
            .ReturnsAsync(
                EitherExtensions.Success<StoreFetchingError, IntIdOutputModel>(idOutputModel)
            );

        // Act
        var result = await _storeController.DeleteStoreAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualIdOutputModel = Assert.IsType<IntIdOutputModel>(okResult.Value);
        actualIdOutputModel.Should().BeEquivalentTo(idOutputModel);
    }

    [Fact]
    public async Task DeleteStoreAsync_WithInvalidStoreId_ReturnsNotFound()
    {
        //Arrange
        _storeServiceMock
            .Setup(service => service.DeleteStoreAsync(1))
            .ReturnsAsync(
                EitherExtensions.Failure<StoreFetchingError, IntIdOutputModel>(
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
*/