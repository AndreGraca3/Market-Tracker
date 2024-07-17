using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.List.ListEntry;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.List;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;

namespace market_tracker_webapi_test.Application.Repository.List;


public class ListEntryRepositoryTest
{
    [Fact]
    public async Task GetListEntriesAsync_ShouldReturnListEntries()
    {
        var listEntryEntities = new List<ListEntryEntity>
        {
            new ListEntryEntity
            {
                ListId = "1",
                ProductId = "1",
                StoreId = 1
            },
            new ListEntryEntity
            {
                ListId = "1",
                ProductId = "2",
                StoreId = 2
            }
        };
        
        var productEntities = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = "1",
                BrandId = 1,
                CategoryId = 1,
                Name = "Product 1",
                Quantity = 1,
                Unit = "unidades",
                Views = 1,
                Rating = 1,
                ImageUrl = "image_url1"
            },
            new ProductEntity
            {
                Id = "2",
                BrandId = 2,
                CategoryId = 2,
                Name = "Product 2",
                Quantity = 2,
                Unit = "unidades",
                Views = 2,
                Rating = 2,
                ImageUrl = "image_url2"
            }
        };
        
        var brandEntities = new List<BrandEntity>
        {
            new BrandEntity
            {
                Id = 1,
                Name = "Brand 1"
            },
            new BrandEntity
            {
                Id = 2,
                Name = "Brand 2"
            }
        };
        
        var categoryEntities = new List<CategoryEntity>
        {
            new CategoryEntity
            {
                Id = 1,
                Name = "Category 1"
            },
            new CategoryEntity
            {
                Id = 2,
                Name = "Category 2"
            }
        };
        
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CityId = 1,
                CompanyId = 1
            },
            new StoreEntity
            {
                Id = 2,
                Name = "Store 2",
                Address = "Address 2",
                OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CityId = 2,
                CompanyId = 2
            }
        };
        
        var context = DbHelper.CreateDatabase(listEntryEntities, productEntities, brandEntities, categoryEntities, storeEntities);
        
        var repository = new ListEntryRepository(context);
        
        // Act
        var listEntries = await repository.GetListEntriesAsync("1", 1);
            
        // Assert
        var expectedProduct1 = new Product(
            new ProductId("1"),
            "Product 1",
            "image_url1",
            1,
            ProductUnit.Units,
            1,
            1,
            new Brand(1, "Brand 1"),
            new Category(new CategoryId(1), "Category 1")
        );
        
        var expectedListEntries = new List<ListEntry>
        {
            new("1", expectedProduct1, new StoreItem(1, "Store 1", "Address 1", 1, 1, Guid.Parse("00000000-0000-0000-0000-000000000001")), 0),
        };
        
        listEntries.Should().BeEquivalentTo(expectedListEntries, x => x.Excluding(y => y.Id));
    }

    // [Fact]
    // public async Task GetListEntryByIdAsync_ShouldReturnListEntry()
    // { 
    //     var listEntryEntities = new List<ListEntryEntity>
    //     {
    //         new ListEntryEntity
    //         {
    //             Quantity = 0,
    //             ListId = "1",
    //             ProductId = "1",
    //             StoreId = 1
    //         },
    //         new ListEntryEntity
    //         {
    //             Quantity = 0,
    //             ListId = "1",
    //             ProductId = "2",
    //             StoreId = 2
    //         }
    //     };
    //     
    //     var productEntities = new List<ProductEntity>
    //     {
    //         new ProductEntity
    //         {
    //             Id = "1",
    //             BrandId = 1,
    //             CategoryId = 1,
    //             Name = "Product 1",
    //             Quantity = 1,
    //             Unit = "unidades",
    //             Views = 1,
    //             Rating = 1,
    //             ImageUrl = "image_url1"
    //         },
    //         new ProductEntity
    //         {
    //             Id = "2",
    //             BrandId = 2,
    //             CategoryId = 2,
    //             Name = "Product 2",
    //             Quantity = 2,
    //             Unit = "unidades",
    //             Views = 2,
    //             Rating = 2,
    //             ImageUrl = "image_url2"
    //         }
    //     };
    //     
    //     var brandEntities = new List<BrandEntity>
    //     {
    //         new BrandEntity
    //         {
    //             Id = 1,
    //             Name = "Brand 1"
    //         },
    //         new BrandEntity
    //         {
    //             Id = 2,
    //             Name = "Brand 2"
    //         }
    //     };
    //     
    //     var categoryEntities = new List<CategoryEntity>
    //     {
    //         new CategoryEntity
    //         {
    //             Id = 1,
    //             Name = "Category 1"
    //         },
    //         new CategoryEntity
    //         {
    //             Id = 2,
    //             Name = "Category 2"
    //         }
    //     };
    //     
    //     var storeEntities = new List<StoreEntity>
    //     {
    //         new StoreEntity
    //         {
    //             Id = 1,
    //             Name = "Store 1",
    //             Address = "Address 1",
    //             OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
    //             CityId = 1,
    //             CompanyId = 1
    //         },
    //         new StoreEntity
    //         {
    //             Id = 2,
    //             Name = "Store 2",
    //             Address = "Address 2",
    //             OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
    //             CityId = 2,
    //             CompanyId = 2
    //         }
    //     };
    //     
    //     var context = DbHelper.CreateDatabase(listEntryEntities, productEntities, brandEntities, categoryEntities, storeEntities);
    //     
    //     var repository = new ListEntryRepository(context);
    //     
    //     // Act
    //     var listEntry = await repository.GetListEntryByIdAsync("1");
    //     
    //     // Assert
    //     var expectedProduct1 = new Product(
    //         new ProductId("1"),
    //         "Product 1",
    //         "image_url1",
    //         1,
    //         ProductUnit.Units,
    //         1,
    //         1,
    //         new Brand(1, "Brand 1"),
    //         new Category(new CategoryId(1), "Category 1")
    //     );
    //     
    //     var expectedListEntry = new ListEntry("1", expectedProduct1, new StoreItem(1, "Store 1", "Address 1", 1, 1, Guid.Parse("00000000-0000-0000-0000-000000000001")), 0);
    //
    //     listEntry.Should().BeEquivalentTo(expectedListEntry, x => x.Excluding(y => y.Id));
    // }

    [Fact]
    public async Task GetListEntryByProductIdAsync_ShouldReturnsListEntry()
    {
        var listEntryEntities = new List<ListEntryEntity>
        {
            new ListEntryEntity
            {
                ListId = "1",
                ProductId = "1",
                StoreId = 1
            },
            new ListEntryEntity
            {
                ListId = "1",
                ProductId = "2",
                StoreId = 2
            }
        };
        
        var productEntities = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = "1",
                BrandId = 1,
                CategoryId = 1,
                Name = "Product 1",
                Quantity = 1,
                Unit = "unidades",
                Views = 1,
                Rating = 1,
                ImageUrl = "image_url1"
            },
            new ProductEntity
            {
                Id = "2",
                BrandId = 2,
                CategoryId = 2,
                Name = "Product 2",
                Quantity = 2,
                Unit = "unidades",
                Views = 2,
                Rating = 2,
                ImageUrl = "image_url2"
            }
        };
        
        var brandEntities = new List<BrandEntity>
        {
            new BrandEntity
            {
                Id = 1,
                Name = "Brand 1"
            },
            new BrandEntity
            {
                Id = 2,
                Name = "Brand 2"
            }
        };
        
        var categoryEntities = new List<CategoryEntity>
        {
            new CategoryEntity
            {
                Id = 1,
                Name = "Category 1"
            },
            new CategoryEntity
            {
                Id = 2,
                Name = "Category 2"
            }
        };
        
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CityId = 1,
                CompanyId = 1
            },
            new StoreEntity
            {
                Id = 2,
                Name = "Store 2",
                Address = "Address 2",
                OperatorId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CityId = 2,
                CompanyId = 2
            }
        };
        
        var context = DbHelper.CreateDatabase(listEntryEntities, productEntities, brandEntities, categoryEntities, storeEntities);
        
        var repository = new ListEntryRepository(context);
        
        // Act
        var listEntry = await repository.GetListEntryByProductIdAsync("1", "1");
        
        // Assert
        var expectedProduct1 = new Product(
            new ProductId("1"),
            "Product 1",
            "image_url1",
            1,
            ProductUnit.Units,
            1,
            1,
            new Brand(1, "Brand 1"),
            new Category(new CategoryId(1), "Category 1")
        );
        
        var expectedListEntry = new ListEntry("1", expectedProduct1, new StoreItem(1, "Store 1", "Address 1", 1, 1, Guid.Parse("00000000-0000-0000-0000-000000000001")), 0);

        listEntry.Should().BeEquivalentTo(expectedListEntry, x => x.Excluding(y => y.Id));
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsListEntryId()
    {
        var context = DbHelper.CreateDatabase();

        var repository = new ListEntryRepository(context);
        
        // Act
        var listEntry = await repository.AddListEntryAsync("1", "1", 1, 1);
        
        // Assert
        var expectedListEntry = new ListEntryEntity()
        {
            ProductId = "1",
            ListId = "1",
            StoreId = 1,
            Quantity = 1
        };

        context.ListEntry.Should().ContainEquivalentOf(expectedListEntry, x => x.Excluding(y => y.Id));
    }

    // [Fact]
    // public async Task UpdateListEntryByIdAsync_ReturnListEntry()
    // {
    //     // Arrange
    //     var listEntryEntities = new List<ListEntryEntity>
    //     {
    //         new ListEntryEntity
    //         {
    //             ListId = "1",
    //             ProductId = "1",
    //             StoreId = 1
    //         },
    //         new ListEntryEntity
    //         {
    //             ListId = "1",
    //             ProductId = "2",
    //             StoreId = 2
    //         }
    //     };
    //
    //     var context = DbHelper.CreateDatabase(listEntryEntities);
    //
    //     var repository = new ListEntryRepository(context);
    //     
    //     // Act
    //     var updatedListEntry = await repository.UpdateListEntryByIdAsync("1", null, 3);
    //     
    //     // Assert
    //     var expectedUpdatedListEntry = new ListEntryEntity
    //     {
    //         ListId = "1",
    //         ProductId = "1",
    //         StoreId = 1,
    //         Quantity = 3
    //     };
    //
    //     updatedListEntry.Should().BeEquivalentTo(expectedUpdatedListEntry);
    // }
    
    // [Fact]
    // public async Task DeleteListEntryByIdAsync_ReturnsListEntry()
    // {
    //     var listEntryEntities = new List<ListEntryEntity>
    //     {
    //         new ListEntryEntity
    //         {
    //             ListId = "1",
    //             ProductId = "1",
    //             StoreId = 1
    //         },
    //         new ListEntryEntity
    //         {
    //             ListId = "1",
    //             ProductId = "2",
    //             StoreId = 2
    //         }
    //     };
    //     
    //     var context = DbHelper.CreateDatabase(listEntryEntities);
    //     
    //     var repository = new ListEntryRepository(context);
    //     
    //     // Act
    //     var deletedListEntry = await repository.DeleteListEntryByIdAsync("1");
    //     
    //     // Assert
    //     var expectedDeletedListEntry = new ListEntryEntity
    //     {
    //         ListId = "1",
    //         ProductId = "1",
    //         StoreId = 1
    //     };
    //
    //     context.ListEntry.Should().NotContainEquivalentOf(expectedDeletedListEntry);
    // }
}
