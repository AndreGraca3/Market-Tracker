using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.List;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Operations.List;
using market_tracker_webapi.Application.Service.Results;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class ListServiceTest
{
    private readonly Mock<IListRepository> _listRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly ListService _listService;

    public ListServiceTest()
    {
        _listRepositoryMock = new Mock<IListRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();

        _listService = new ListService(
            _listRepositoryMock.Object,
            _clientRepositoryMock.Object,
            new MockedTransactionManager()
        );
    }

    [Fact]
    public async Task GetListsAsync_ReturnsCollectionOutputModel()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListsFromClientAsync(
                It.IsAny<Guid>(), null, null, null, null)
            ).ReturnsAsync(MockedData.DummyLists);

        // Act
        var result = await _listService
            .GetListsAsync(It.IsAny<Guid>(), null, null, null, null);

        // Assert
        result.Should().BeEquivalentTo(MockedData.DummyLists);
    }

    [Fact]
    public async Task GetListByIdAsync_ReturnsList()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listRepositoryMock
            .Setup(x => x.GetClientMembersByListIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0].MemberIds
                .Select(id => MockedData.DummyClients.First(c => c.Id == id).ToClientItem()));

        _clientRepositoryMock
            .Setup(x => x.GetClientByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(MockedData.DummyClients[0]);

        // Act
        var result =
            await _listService.GetListByIdAsync(MockedData.DummyLists[0].Id.Value, MockedData.DummyLists[0].OwnerId.Value);

        // Assert
        result.Should().BeEquivalentTo(new ShoppingListResult(
            MockedData.DummyLists[0].Id.Value,
            MockedData.DummyLists[0].Name,
            MockedData.DummyLists[0].ArchivedAt,
            MockedData.DummyLists[0].CreatedAt,
            MockedData.DummyClients[0].ToClientItem(),
            MockedData.DummyLists[0].MemberIds.Select(id =>
                MockedData.DummyClients.First(c => c.Id == id).ToClientItem())
        ));
    }

    [Fact]
    public async Task GetListByIdAsync_ReturnsListByIdNotFound()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ShoppingList)null!);

        // Act
        var ex = await Assert.ThrowsAsync<MarketTrackerServiceException>(
            async () => await _listService.GetListByIdAsync(It.IsAny<string>(), It.IsAny<Guid>()));

        // Assert
        ex.ServiceError
            .Should()
            .BeEquivalentTo(new ListFetchingError.ListByIdNotFound(It.IsAny<string>()));
    }

    [Fact]
    public async Task AddListAsync_ReturnsId()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListsFromClientAsync(
                It.IsAny<Guid>(), true, null, null, null))
            .ReturnsAsync(new List<ShoppingList>());

        _listRepositoryMock
            .Setup(x => x.AddListAsync(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync(MockedData.DummyLists[0].Id);

        // Act
        var result = await _listService.AddListAsync(It.IsAny<Guid>(), It.IsAny<string>());

        // Assert
        result.Should().BeEquivalentTo(MockedData.DummyLists[0].Id);
    }

    [Fact]
    public async Task AddListAsync_ReturnsMaxListNumberReached()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListsFromClientAsync(
                It.IsAny<Guid>(), true, null, null, null))
            .ReturnsAsync(Enumerable.Repeat(MockedData.DummyLists[0], ListService.MaxListNumber));

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listService.AddListAsync(It.IsAny<Guid>(), "newList"));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(
                new ListCreationError.MaxListNumberReached(It.IsAny<Guid>(), ListService.MaxListNumber)
            );
    }

    [Fact]
    public async Task AddListAsync_ReturnsListNameAlreadyExists()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListsFromClientAsync(
                It.IsAny<Guid>(),
                It.IsAny<bool?>(),
                It.IsAny<string?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<bool?>()
            ))
            .ReturnsAsync(MockedData.DummyLists);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listService.AddListAsync(It.IsAny<Guid>(), It.IsAny<string>())
        );

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListCreationError.ListNameAlreadyExists(It.IsAny<Guid>(), It.IsAny<string>()));
    }

    [Fact]
    public async Task UpdateListAsync_ReturnsShoppingList()
    {
        // Arrange
        var newList = new ShoppingListItem(
            MockedData.DummyLists[0].Id.Value,
            "List 1 updated",
            MockedData.DummyLists[0].ArchivedAt,
            MockedData.DummyLists[0].CreatedAt,
            MockedData.DummyLists[0].OwnerId.Value
        );

        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listRepositoryMock
            .Setup(x =>
                x.UpdateListAsync(It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<string?>()))
            .ReturnsAsync(newList);

        // Act
        var result =
            await _listService.UpdateListAsync(newList.Id, newList.OwnerId, newList.Name, null);

        // Assert
        result.Should().BeEquivalentTo(newList);
    }

    [Fact]
    public async Task UpdateListAsync_ReturnsListByIdNotFound()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ShoppingList)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listService.UpdateListAsync(
                It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string?>(), null));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListFetchingError.ListByIdNotFound(It.IsAny<string>()));
    }

    [Fact]
    public async Task UpdateListAsync_ReturnsUserDoesNotOwnList()
    {
        // 
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listService.UpdateListAsync(MockedData.DummyLists[0].Id.Value, MockedData.DummyClients[1].Id.Value,
                It.IsAny<string?>(), null));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(
                new ListFetchingError.ClientDoesNotBelongToList(MockedData.DummyClients[1].Id.Value,
                    MockedData.DummyLists[0].Id.Value));
    }

    [Fact]
    public async Task UpdateListAsync_ReturnsListIsArchived()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[1]);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listService.UpdateListAsync(MockedData.DummyLists[1].Id.Value, MockedData.DummyLists[1].OwnerId.Value,
                It.IsAny<string?>(), true));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListUpdateError.ListIsArchived(MockedData.DummyLists[1].Id.Value));
    }

    [Fact]
    public async Task DeleteListAsync_ReturnsShoppingList()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listRepositoryMock
            .Setup(x => x.DeleteListAsync(It.IsAny<string>()))
            .ReturnsAsync(new ShoppingListItem(
                MockedData.DummyLists[0].Id.Value,
                MockedData.DummyLists[0].Name,
                MockedData.DummyLists[0].ArchivedAt,
                MockedData.DummyLists[0].CreatedAt,
                MockedData.DummyLists[0].OwnerId.Value
            ));

        // Act
        var result =
            await _listService.DeleteListAsync(MockedData.DummyLists[0].Id.Value, MockedData.DummyLists[0].OwnerId.Value);

        // Assert
        result.Should().BeEquivalentTo(new ShoppingListItem(
            MockedData.DummyLists[0].Id.Value,
            MockedData.DummyLists[0].Name,
            MockedData.DummyLists[0].ArchivedAt,
            MockedData.DummyLists[0].CreatedAt,
            MockedData.DummyLists[0].OwnerId.Value
        ));
    }

    [Fact]
    public async Task DeleteListAsync_ReturnsListByIdNotFound()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ShoppingList)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listService.DeleteListAsync(It.IsAny<string>(), It.IsAny<Guid>()));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListFetchingError.ListByIdNotFound(It.IsAny<string>()));
    }
}