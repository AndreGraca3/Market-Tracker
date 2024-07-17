using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Repository.Account.Token;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Auth;

namespace market_tracker_webapi_test.Application.Repository.Account;

public class TokenRepositoryTest
{
    [Fact]
    public async Task GetTokenByTokenValueAsync_WhenTokenExists_ReturnsToken()
    {
        // Arrange
        var tokenEntities = new List<TokenEntity>
        {
            new TokenEntity
            {
                TokenValue = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };

        var context = DbHelper.CreateDatabase(tokenEntities);
        
        var tokenRepository = new TokenRepository(context);
        
        // Act
        var actual = await tokenRepository.GetTokenByTokenValueAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedToken = new Token(Guid.Parse("00000000-0000-0000-0000-000000000001"),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2),
            Guid.Parse("00000000-0000-0000-0000-000000000002"));

        actual.Should().BeEquivalentTo(expectedToken, x => x
            .Excluding(y => y.ExpiresAt)
            .Excluding(y => y.CreatedAt)
        );
    }
    
    [Fact]
    public async Task GetTokenByTokenValueAsync_WhenTokenDoesNotExist_ReturnsNull()
    {
        // Arrange
        var tokenEntities = new List<TokenEntity>
        {
            new TokenEntity
            {
                TokenValue = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };

        var context = DbHelper.CreateDatabase(tokenEntities);
        
        var tokenRepository = new TokenRepository(context);
        
        // Act
        var actual = await tokenRepository.GetTokenByTokenValueAsync(Guid.Parse("00000000-0000-0000-0000-000000000003"));
        
        // Assert
        actual.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateTokenAsync_ReturnsToken()
    {
        // Arrange
        var tokenEntities = new List<TokenEntity>
        {
            new TokenEntity
            {
                TokenValue = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };

        var context = DbHelper.CreateDatabase(tokenEntities);
        
        var tokenRepository = new TokenRepository(context);
        
        // Act
        var actual = await tokenRepository.CreateTokenAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"));
        
        // Assert
        var expectedToken = new Token(Guid.Parse("00000000-0000-0000-0000-000000000001"),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2),
            Guid.Parse("00000000-0000-0000-0000-000000000002"));

        actual.Should().BeEquivalentTo(expectedToken, x => x
            .Excluding(y => y.ExpiresAt)
            .Excluding(y => y.CreatedAt)
            .Excluding(y => y.Value)
        );
    }
    
    [Fact]
    public async Task CreateTokenByTokenValueAsync_ReturnsTokenValue()
    {
        // Arrange
        var tokenEntities = new List<TokenEntity>
        {
            new TokenEntity
            {
                TokenValue = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };

        var context = DbHelper.CreateDatabase(tokenEntities);
        
        var tokenRepository = new TokenRepository(context);
        
        // Act
        var actual = await tokenRepository.CreateTokenByTokenValueAsync(Guid.Parse("00000000-0000-0000-0000-000000000003"), DateTime.UtcNow.AddDays(1), Guid.Parse("00000000-0000-0000-0000-000000000002"));
        
        // Assert
        actual.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000003"));
    }
    
    [Fact]
    public async Task GetTokenByUserIdAsync_WhenTokenExists_ReturnsToken()
    {
        // Arrange
        var tokenEntities = new List<TokenEntity>
        {
            new TokenEntity
            {
                TokenValue = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };

        var context = DbHelper.CreateDatabase(tokenEntities);
        
        var tokenRepository = new TokenRepository(context);
        
        // Act
        var actual = await tokenRepository.GetTokenByUserIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"));
        
        // Assert
        var expectedToken = new Token(Guid.Parse("00000000-0000-0000-0000-000000000001"),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2),
            Guid.Parse("00000000-0000-0000-0000-000000000002"));

        actual.Should().BeEquivalentTo(expectedToken, x => x
            .Excluding(y => y.ExpiresAt)
            .Excluding(y => y.CreatedAt)
        );
    }
    
    [Fact]
    public async Task GetTokenByUserIdAsync_WhenTokenDoesNotExist_ReturnsNull()
    {
        // Arrange
        var tokenEntities = new List<TokenEntity>
        {
            new TokenEntity
            {
                TokenValue = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };

        var context = DbHelper.CreateDatabase(tokenEntities);
        
        var tokenRepository = new TokenRepository(context);
        
        // Act
        var actual = await tokenRepository.GetTokenByUserIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000003"));
        
        // Assert
        actual.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteTokenAsync_WhenTokenExists_ReturnsToken()
    {
        // Arrange
        var tokenEntities = new List<TokenEntity>
        {
            new TokenEntity
            {
                TokenValue = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };

        var context = DbHelper.CreateDatabase(tokenEntities);
        
        var tokenRepository = new TokenRepository(context);
        
        // Act
        var actual = await tokenRepository.DeleteTokenAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        
        // Assert
        var expectedToken = new Token(Guid.Parse("00000000-0000-0000-0000-000000000001"),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2),
            Guid.Parse("00000000-0000-0000-0000-000000000002"));

        actual.Should().BeEquivalentTo(expectedToken, x => x
            .Excluding(y => y.ExpiresAt)
            .Excluding(y => y.CreatedAt)
        );
    }
    
    [Fact]
    public async Task DeleteTokenAsync_WhenTokenDoesNotExist_ReturnsNull()
    {
        // Arrange
        var tokenEntities = new List<TokenEntity>
        {
            new TokenEntity
            {
                TokenValue = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002")
            }
        };

        var context = DbHelper.CreateDatabase(tokenEntities);
        
        var tokenRepository = new TokenRepository(context);
        
        // Act
        var actual = await tokenRepository.DeleteTokenAsync(Guid.Parse("00000000-0000-0000-0000-000000000003"));
        
        // Assert
        actual.Should().BeNull();
    }
}