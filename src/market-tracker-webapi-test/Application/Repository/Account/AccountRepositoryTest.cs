using FluentAssertions;
using market_tracker_webapi.Application.Repository.Account.Credential;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Auth;

namespace market_tracker_webapi_test.Application.Repository.Account;

public class AccountRepositoryTest
{
    [Fact]
    public async Task GetPasswordByUserIdAsync_ReturnsAccount()
    {
        // Arrange
        var accountEntities = new List<AccountEntity>
        {
            new AccountEntity
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password"
            }
        };

        var context = DbHelper.CreateDatabase(accountEntities);
        
        var accountRepository = new AccountRepository(context);
        
        // Act
        var account = await accountRepository.GetPasswordByUserIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedAccount = new market_tracker_webapi.Application.Domain.Schemas.Account.Auth.Account(Guid.Parse("00000000-0000-0000-0000-000000000001"), "password");

        account.Should().BeEquivalentTo(expectedAccount);
    }
    
    [Fact]
    public async Task GetPasswordByUserIdAsync_ReturnsNull()
    {
        // Arrange
        var accountEntities = new List<AccountEntity>
        {
            new AccountEntity
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password"
            }
        };

        var context = DbHelper.CreateDatabase(accountEntities);
        
        var accountRepository = new AccountRepository(context);
        
        // Act
        var account = await accountRepository.GetPasswordByUserIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"));
        
        // Assert
        account.Should().BeNull();
    }
    
    [Fact]
    public async Task CreatePasswordAsync_ReturnsAccount()
    {
        // Arrange
        var accountEntities = new List<AccountEntity>
        {
            new AccountEntity
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password"
            }
        };

        var context = DbHelper.CreateDatabase(accountEntities);
        
        var accountRepository = new AccountRepository(context);
        
        // Act
        var account = await accountRepository.CreatePasswordAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"), "password");
        
        // Assert
        var expectedAccount = new market_tracker_webapi.Application.Domain.Schemas.Account.Auth.Account(Guid.Parse("00000000-0000-0000-0000-000000000002"), "password");

        account.Should().BeEquivalentTo(expectedAccount);
    }
    
    [Fact]
    public async Task UpdatePasswordAsync_ReturnsAccount()
    {
        // Arrange
        var accountEntities = new List<AccountEntity>
        {
            new AccountEntity
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password"
            }
        };

        var context = DbHelper.CreateDatabase(accountEntities);
        
        var accountRepository = new AccountRepository(context);
        
        // Act
        var account = await accountRepository.UpdatePasswordAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"), "newPassword");
        
        // Assert
        var expectedAccount = new market_tracker_webapi.Application.Domain.Schemas.Account.Auth.Account(Guid.Parse("00000000-0000-0000-0000-000000000001"), "newPassword");

        account.Should().BeEquivalentTo(expectedAccount);
    }
    
    [Fact]
    public async Task UpdatePasswordAsync_ReturnsNull()
    {
        // Arrange
        var accountEntities = new List<AccountEntity>
        {
            new AccountEntity
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password"
            }
        };

        var context = DbHelper.CreateDatabase(accountEntities);
        
        var accountRepository = new AccountRepository(context);
        
        // Act
        var account = await accountRepository.UpdatePasswordAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"), "newPassword");
        
        // Assert
        account.Should().BeNull();
    }
    
    [Fact]
    public async Task DeletePasswordAsync_ReturnsAccount()
    {
        // Arrange
        var accountEntities = new List<AccountEntity>
        {
            new AccountEntity
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password"
            }
        };

        var context = DbHelper.CreateDatabase(accountEntities);
        
        var accountRepository = new AccountRepository(context);
        
        // Act
        var account = await accountRepository.DeletePasswordAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedAccount = new market_tracker_webapi.Application.Domain.Schemas.Account.Auth.Account(Guid.Parse("00000000-0000-0000-0000-000000000001"), "password");

        account.Should().BeEquivalentTo(expectedAccount);
    }
    
    [Fact]
    public async Task DeletePasswordAsync_ReturnsNull()
    {
        // Arrange
        var accountEntities = new List<AccountEntity>
        {
            new AccountEntity
            {
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Password = "password"
            }
        };

        var context = DbHelper.CreateDatabase(accountEntities);
        
        var accountRepository = new AccountRepository(context);
        
        // Act
        var account = await accountRepository.DeletePasswordAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"));
        
        // Assert
        account.Should().BeNull();
    }
}