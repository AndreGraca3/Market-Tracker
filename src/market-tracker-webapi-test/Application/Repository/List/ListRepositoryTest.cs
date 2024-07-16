using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Repository.List;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.List;

namespace market_tracker_webapi_test.Application.Repository.List;


public class ListRepositoryTest
{
    // [Fact]
    // public async Task GetListsFromClientAsync_ShouldReturnLists()
    // {
    //     // Arrange
    //     var listsEntites = new List<ListEntity>
    //     {
    //         new ListEntity
    //         {
    //             Id = "1",
    //             Name = "List 1",
    //             ArchivedAt = null,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
    //         },
    //         new ListEntity
    //         {
    //             Id = "2",
    //             Name = "List 2",
    //             ArchivedAt = null,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000002")
    //         }
    //     };
    //     
    //     var context = DbHelper.CreateDatabase(listsEntites);
    //     
    //     var listRepository = new ListRepository(context);
    //     
    //     // Act
    //     var actual = await listRepository.GetListsFromClientAsync(
    //         clientId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
    //         isOwner: true,
    //         listName: "List 1",
    //         createdAfter: new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //         isArchived: false
    //     );
    //     
    //     // Assert
    //     var expectLists = new List<ShoppingList>
    //     {
    //         new(
    //             Id: "1",
    //             Name: "List 1",
    //             ArchivedAt: null,
    //             CreatedAt: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             OwnerId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
    //             MemberIds: new List<Guid>()
    //             )
    //     };
    //     actual.Should().BeEquivalentTo(expectLists);
    //}
    
    [Fact]
    public async Task GetClientMembersByListIdAsync_ShouldReturnClientMembers()
    {
        // Arrange
        var listClientEntites = new List<ListClientEntity>
        {
            new ListClientEntity
            {
                ListId = "1",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            },
            new ListClientEntity
            {
                ListId = "1",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };
        
        var clientEntities = new List<ClientEntity>
        {
            new ClientEntity
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Username = "user1",
                Avatar = null
            },
            new ClientEntity
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Username = "user2",
                Avatar = null
            }
        }; 
        
        var context = DbHelper.CreateDatabase(listClientEntites, clientEntities);
        
        var listRepository = new ListRepository(context);
        
        // Act
        var actual = await listRepository.GetClientMembersByListIdAsync("1");
        
        // Assert
        var expectMembers = new List<ClientItem>
        {
            new(Guid.Parse("00000000-0000-0000-0000-000000000001"), "user1", null),
            new(Guid.Parse("00000000-0000-0000-0000-000000000002"), "user2", null)
        };
        actual.Should().BeEquivalentTo(expectMembers);
    }
    
    [Fact]
    public async Task GetListByIdAsync_ShouldReturnList()
    {
        // Arrange
        var listEntities = new List<ListEntity>
        {
            new ListEntity
            {
                Id = "1",
                Name = "List 1",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            }
        };
        
        var listClientEntities = new List<ListClientEntity>
        {
            new ListClientEntity
            {
                ListId = "1",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001")
            },
            new ListClientEntity
            {
                ListId = "1",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };
        
        var context = DbHelper.CreateDatabase(listEntities, listClientEntities);
        
        var listRepository = new ListRepository(context);
        
        // Act
        var actual = await listRepository.GetListByIdAsync("1");
        
        // Assert
        var expectList = new ShoppingList(
            Id: "1",
            Name: "List 1",
            ArchivedAt: null,
            CreatedAt: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            OwnerId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            MemberIds: new List<Guid>
            {
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        );
        actual.Should().BeEquivalentTo(expectList);
    }
    
    [Fact]
    public async Task AddListAsync_ShouldReturnShoppingListId()
    {
        // Arrange
        var context = DbHelper.CreateDatabase();
        
        var listRepository = new ListRepository(context);
        
        // Act
        var actual = await listRepository.AddListAsync("List 1", Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedAddedListEntity = new ListEntity()
        {
            Name = "List 1",
            OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            CreatedAt = DateTime.Now
        };
        context.List.Should().ContainEquivalentOf(expectedAddedListEntity, 
            x => x
                .Excluding(y => y.CreatedAt)
                .Excluding(y => y.Id)
                .Excluding(y => y.ArchivedAt)
            );
    }

    [Fact]
    public async Task UpdateListAsync_ShouldReturnUpdateShoppingListItem()
    {
        
    }
}
