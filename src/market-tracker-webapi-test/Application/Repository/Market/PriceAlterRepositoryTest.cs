using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Repository.Market.Alert;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repository.Market;

public class PriceAlterRepositoryTest
{
    [Fact]
    public async Task GetPriceAlertsAsync_ShouldReturnPriceAlerts()
    {
        // Arrange
        var alertEntites = new List<PriceAlertEntity>
        {
            new PriceAlertEntity
            {
                Id = "1",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ProductId = "product1",
                StoreId = 1,
                PriceThreshold = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new PriceAlertEntity
            {
                Id = "2",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ProductId = "product2",
                StoreId = 2,
                PriceThreshold = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(alertEntites);
        
        var alertRepository = new PriceAlertRepository(context);
        
        // Act
        var actual = await alertRepository.GetPriceAlertsAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "product1",
            1,
            100);
        
        // Assert
        var expectAlerts = new List<PriceAlert>
        {
            new(id: "1", clientId: Guid.Parse("00000000-0000-0000-0000-000000000001"), productId: "product1", storeId: 1, priceThreshold: 100, createdAt: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified))
        };
        actual.Should().BeEquivalentTo(expectAlerts);
    }
    
    [Fact]
    public async Task GetPriceAlertAsync_ShouldReturnPriceAlert()
    {
        // Arrange
        var alertEntites = new List<PriceAlertEntity>
        {
            new PriceAlertEntity
            {
                Id = "1",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ProductId = "product1",
                StoreId = 1,
                PriceThreshold = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new PriceAlertEntity
            {
                Id = "2",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ProductId = "product2",
                StoreId = 2,
                PriceThreshold = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(alertEntites);
        
        var alertRepository = new PriceAlertRepository(context);
        
        // Act
        var actual = await alertRepository.GetPriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "product1",
            1);
        
        // Assert
        var expectAlert = new PriceAlert(
            id: "1",
            clientId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            productId: "product1",
            storeId: 1,
            priceThreshold: 100,
            createdAt: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );
        actual.Should().BeEquivalentTo(expectAlert);
    }
    
    [Fact]
    public async Task AddPriceAlertAsync_ShouldReturnPriceAlertId()
    {
        // Arrange
        var alertEntites = new List<PriceAlertEntity>
        {
            new PriceAlertEntity
            {
                Id = "1",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ProductId = "product1",
                StoreId = 1,
                PriceThreshold = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new PriceAlertEntity
            {
                Id = "2",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ProductId = "product2",
                StoreId = 2,
                PriceThreshold = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(alertEntites);
        
        var alertRepository = new PriceAlertRepository(context);
        
        // Act
        var actual = await alertRepository.AddPriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "product1",
            1,
            100);
        
        // Assert
        var expectedAddedAlert = new PriceAlertEntity
        {
            Id = "3",
            ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            ProductId = "product1",
            StoreId = 1,
            PriceThreshold = 100,
            CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        };
        
        context.PriceAlert.Should().ContainEquivalentOf(expectedAddedAlert, x => x.Excluding(y => y.Id));
    }
    
    [Fact]
    public async Task RemovePriceAlertByIdAsync_ShouldReturnPriceAlert()
    {
        // Arrange
        var alertEntites = new List<PriceAlertEntity>
        {
            new PriceAlertEntity
            {
                Id = "1",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ProductId = "product1",
                StoreId = 1,
                PriceThreshold = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new PriceAlertEntity
            {
                Id = "2",
                ClientId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ProductId = "product2",
                StoreId = 2,
                PriceThreshold = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(alertEntites);
        
        var alertRepository = new PriceAlertRepository(context);
        
        // Act
        var actual = await alertRepository.RemovePriceAlertByIdAsync("1");
        
        // Assert
        var expectAlert = new PriceAlert(
            id: "1",
            clientId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            productId: "product1",
            storeId: 1,
            priceThreshold: 100,
            createdAt: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );
        actual.Should().BeEquivalentTo(expectAlert);
    }
}