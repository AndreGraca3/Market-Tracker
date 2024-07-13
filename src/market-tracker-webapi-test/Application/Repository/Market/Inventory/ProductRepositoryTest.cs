using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace market_tracker_webapi_test.Application.Repository.Market.Inventory;

public class ProductRepositoryTest
{
    // [Fact]
    // public async Task GetAvailableProductsAsync_ReturnsListAsync()
    // {
    //     // Arrange
    //     var dbProductEntities = new List<ProductEntity>
    //     {
    //         new()
    //         {
    //             Id = "1",
    //             Name = "Banana",
    //             BrandId = 1,
    //             CategoryId = 1,
    //             ImageUrl = "1",
    //             Quantity = 1,
    //             Rating = 1.0,
    //             Unit = "unidadades",
    //             Views = 1
    //         },
    //         new()
    //         {
    //             Id = "2",
    //             Name = "Apple",
    //             BrandId = 2,
    //             CategoryId = 2,
    //             ImageUrl = "2",
    //             Quantity = 2,
    //             Rating = 2.0,
    //             Unit = "unidades",
    //             Views = 2
    //         }
    //     };
    //
    //     var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
    //     {
    //         new()
    //         {
    //             ProductId = "1",
    //             StoreId = 1,
    //             IsAvailable = true,
    //             LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         },
    //         new()
    //         {
    //             ProductId = "2",
    //             StoreId = 2,
    //             IsAvailable = false,
    //             LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         }
    //     };
    //
    //     var dbStoreEntities = new List<StoreEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Store1",
    //             CompanyId = 1,
    //             CityId = 1,
    //             Address = "Address1",
    //             OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Store2",
    //             CompanyId = 2,
    //             CityId = 2,
    //             Address = "Address2",
    //             OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302")
    //         }
    //     };
    //
    //     var dbPriceEntryEntities = new List<PriceEntryEntity>
    //     {
    //         new()
    //         {
    //             Id = "1",
    //             ProductId = "1",
    //             StoreId = 1,
    //             Price = 100,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         },
    //         new()
    //         {
    //             Id = "2",
    //             ProductId = "2",
    //             StoreId = 2,
    //             Price = 200,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         },
    //     };
    //
    //     var dbBrandEntities = new List<BrandEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Brand1"
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Brand2"
    //         }
    //     };
    //
    //     var dbCategoryEntities = new List<CategoryEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Category1"
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Category2"
    //         }
    //     };
    //
    //     var dbCompanyEntities = new List<CompanyEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Company1",
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             LogoUrl = "1",
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Company2",
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             LogoUrl = "2",
    //         }
    //     };
    //
    //     var dbCityEntities = new List<CityEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "City1"
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "City2"
    //         }
    //     };
    //
    //     var dbPromotionEntities = new List<PromotionEntity>
    //     {
    //         new()
    //         {
    //             Percentage = 20,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             PriceEntryId = "1"
    //         },
    //         new()
    //         {
    //             Percentage = 30,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             PriceEntryId = "2"
    //         }
    //     };
    //
    //
    //
    //
    //     var context = CreateDatabase(
    //         dbProductEntities,
    //         dbProductAvailabilityEntities,
    //         dbStoreEntities,
    //         dbPriceEntryEntities,
    //         dbBrandEntities,
    //         dbCategoryEntities,
    //         dbCompanyEntities,
    //         dbCityEntities,
    //         dbPromotionEntities);
    //     var productRepo = new ProductRepository(context);
    //
    //     // Act
    //     var actualAvailableProducts = await productRepo
    //         .GetAvailableProductsAsync(
    //             0,
    //             5,
    //             3,
    //             ProductsSortOption.Relevance,
    //             "Banana",
    //             new List<int>() { 1 },
    //             new List<int>() { 1 },
    //             0,
    //             1000,
    //             0,
    //             10,
    //             new List<int>() { 1 },
    //             new List<int>() { 1 },
    //             new List<int>() { 1 }
    //         );
    //
    //     var paginatedFacetedProducts = new PaginatedFacetedProducts(
    //         new PaginatedResult<Product>(
    //             new List<Product>
    //             {
    //                 new Product(
    //                     "1",
    //                     "Banana",
    //                     "ImageUrl1",
    //                     10,
    //                     ProductUnit.Units,
    //                     100,
    //                     4.5,
    //                     new Brand(1, "BrandName"),
    //                     new Category(1, "CategoryName")
    //                 )
    //             },
    //             1,
    //             0,
    //             5
    //         ),
    //         new ProductsFacetsCounters(
    //             It.IsAny<IEnumerable<FacetCounter>>(),
    //             It.IsAny<IEnumerable<FacetCounter>>(),
    //             It.IsAny<IEnumerable<FacetCounter>>()
    //         )
    //     );
    //
    //     // Assert
    //     actualAvailableProducts.Should().BeEquivalentTo(paginatedFacetedProducts);
    // }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsObjectAsync()
    {
        // Arrange
        var dbProductEntities = new List<ProductEntity>
        {
            new()
            {
                Id = "1",
                Name = "Banana",
                BrandId = 1,
                CategoryId = 1,
                ImageUrl = "1",
                Quantity = 1,
                Rating = 1.0,
                Unit = "unidades",
                Views = 1
            },
            new()
            {
                Id = "2",
                Name = "Apple",
                BrandId = 2,
                CategoryId = 2,
                ImageUrl = "2",
                Quantity = 2,
                Rating = 2.0,
                Unit = "unidades",
                Views = 2
            }
        };

        var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
        {
            new()
            {
                ProductId = "1",
                StoreId = 1,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                ProductId = "2",
                StoreId = 2,
                IsAvailable = false,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        var dbStoreEntities = new List<StoreEntity>
        {
            new()
            {
                Id = 1,
                Name = "Store1",
                CompanyId = 1,
                CityId = 1,
                Address = "Address1",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new()
            {
                Id = 2,
                Name = "Store2",
                CompanyId = 2,
                CityId = 2,
                Address = "Address2",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302")
            }
        };

        var dbPriceEntryEntities = new List<PriceEntryEntity>
        {
            new()
            {
                Id = "1",
                ProductId = "1",
                StoreId = 1,
                Price = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = "2",
                ProductId = "2",
                StoreId = 2,
                Price = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
        };

        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand1"
            },
            new()
            {
                Id = 2,
                Name = "Brand2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category1"
            },
            new()
            {
                Id = 2,
                Name = "Category2"
            }
        };

        var dbCompanyEntities = new List<CompanyEntity>
        {
            new()
            {
                Id = 1,
                Name = "Company1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "1",
            },
            new()
            {
                Id = 2,
                Name = "Company2",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "2",
            }
        };

        var dbCityEntities = new List<CityEntity>
        {
            new()
            {
                Id = 1,
                Name = "City1"
            },
            new()
            {
                Id = 2,
                Name = "City2"
            }
        };

        var dbPromotionEntities = new List<PromotionEntity>
        {
            new()
            {
                Percentage = 20,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "1"
            },
            new()
            {
                Percentage = 30,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "2"
            }
        };

        var context = CreateDatabase(
            dbProductEntities,
            dbProductAvailabilityEntities,
            dbStoreEntities,
            dbPriceEntryEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbCompanyEntities,
            dbCityEntities,
            dbPromotionEntities);
        
        var productRepo = new ProductRepository(context);
        
        // Act
        var actualProduct = await productRepo.GetProductByIdAsync("1");
        
        var expectedProduct =                     
            new Product(
            "1",
            "Banana",
            "1",
            1,
            ProductUnit.Units,
            1,
            1.0,
            new Brand(1, "Brand1"),
            new Category(1, "Category1")
        );

        actualProduct.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task AddProductAsync_ReturnsProductIdAsync()
    {
        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand1"
            },
            new()
            {
                Id = 2,
                Name = "Brand2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category1"
            },
            new()
            {
                Id = 2,
                Name = "Category2"
            }
        };

        var context = CreateDatabase(
            new List<ProductEntity>(),
            new List<ProductAvailabilityEntity>(),
            new List<StoreEntity>(),
            new List<PriceEntryEntity>(),
            dbBrandEntities,
            dbCategoryEntities,
            new List<CompanyEntity>(),
            new List<CityEntity>(),
            new List<PromotionEntity>());
        
        var productRepo = new ProductRepository(context);
        
        var actualProductId = await productRepo.AddProductAsync(
            "1",
            "Banana",
            "1",
            1,
            "unidades",
            1,
            1
        );
        
        actualProductId.Value.Should().Be("1");
    }

    [Fact]
    public async Task SetProductAvailabilityAsync_WhenAvailabilityIsNull_ReturnsTaskAsync()
    {
                        // Arrange
        var dbProductEntities = new List<ProductEntity>
        {
            new()
            {
                Id = "1",
                Name = "Banana",
                BrandId = 1,
                CategoryId = 1,
                ImageUrl = "1",
                Quantity = 1,
                Rating = 1.0,
                Unit = "unidades",
                Views = 1
            },
            new()
            {
                Id = "2",
                Name = "Apple",
                BrandId = 2,
                CategoryId = 2,
                ImageUrl = "2",
                Quantity = 2,
                Rating = 2.0,
                Unit = "unidades",
                Views = 2
            }
        };
        
        var dbStoreEntities = new List<StoreEntity>
        {
            new()
            {
                Id = 1,
                Name = "Store1",
                CompanyId = 1,
                CityId = 1,
                Address = "Address1",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new()
            {
                Id = 2,
                Name = "Store2",
                CompanyId = 2,
                CityId = 2,
                Address = "Address2",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302")
            }
        };

        var dbPriceEntryEntities = new List<PriceEntryEntity>
        {
            new()
            {
                Id = "1",
                ProductId = "1",
                StoreId = 1,
                Price = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = "2",
                ProductId = "2",
                StoreId = 2,
                Price = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
        };

        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand1"
            },
            new()
            {
                Id = 2,
                Name = "Brand2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category1"
            },
            new()
            {
                Id = 2,
                Name = "Category2"
            }
        };

        var dbCompanyEntities = new List<CompanyEntity>
        {
            new()
            {
                Id = 1,
                Name = "Company1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "1",
            },
            new()
            {
                Id = 2,
                Name = "Company2",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "2",
            }
        };

        var dbCityEntities = new List<CityEntity>
        {
            new()
            {
                Id = 1,
                Name = "City1"
            },
            new()
            {
                Id = 2,
                Name = "City2"
            }
        };

        var dbPromotionEntities = new List<PromotionEntity>
        {
            new()
            {
                Percentage = 20,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "1"
            },
            new()
            {
                Percentage = 30,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "2"
            }
        };

        var context = CreateDatabase(
            dbProductEntities,
            new List<ProductAvailabilityEntity>(),
            dbStoreEntities,
            dbPriceEntryEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbCompanyEntities,
            dbCityEntities,
            dbPromotionEntities);
        
        var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
        {
            new()
            {
                ProductId = "1",
                StoreId = 1,
                IsAvailable = true,
                LastChecked = It.IsAny<DateTime>()
            }
        };
        
        var productRepo = new ProductRepository(context);
        
        // Act
        await productRepo.SetProductAvailabilityAsync("1", 1, true);
        
        // Assert
        context.ProductAvailability.Should().BeEquivalentTo(dbProductAvailabilityEntities, x => x.Excluding(y => y.LastChecked));
    }

    [Fact]
    public async Task SetProductAvailabilityAsync_WhenAvailabilityIsNotNull_ReturnsTaskAsync()
    {
        // Arrange
        var dbProductEntities = new List<ProductEntity>
        {
            new()
            {
                Id = "1",
                Name = "Banana",
                BrandId = 1,
                CategoryId = 1,
                ImageUrl = "1",
                Quantity = 1,
                Rating = 1.0,
                Unit = "unidades",
                Views = 1
            },
            new()
            {
                Id = "2",
                Name = "Apple",
                BrandId = 2,
                CategoryId = 2,
                ImageUrl = "2",
                Quantity = 2,
                Rating = 2.0,
                Unit = "unidades",
                Views = 2
            }
        };

        var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
        {
            new()
            {
                ProductId = "1",
                StoreId = 1,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        var dbStoreEntities = new List<StoreEntity>
        {
            new()
            {
                Id = 1,
                Name = "Store1",
                CompanyId = 1,
                CityId = 1,
                Address = "Address1",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new()
            {
                Id = 2,
                Name = "Store2",
                CompanyId = 2,
                CityId = 2,
                Address = "Address2",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302")
            }
        };

        var dbPriceEntryEntities = new List<PriceEntryEntity>
        {
            new()
            {
                Id = "1",
                ProductId = "1",
                StoreId = 1,
                Price = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = "2",
                ProductId = "2",
                StoreId = 2,
                Price = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
        };

        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand1"
            },
            new()
            {
                Id = 2,
                Name = "Brand2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category1"
            },
            new()
            {
                Id = 2,
                Name = "Category2"
            }
        };

        var dbCompanyEntities = new List<CompanyEntity>
        {
            new()
            {
                Id = 1,
                Name = "Company1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "1",
            },
            new()
            {
                Id = 2,
                Name = "Company2",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "2",
            }
        };

        var dbCityEntities = new List<CityEntity>
        {
            new()
            {
                Id = 1,
                Name = "City1"
            },
            new()
            {
                Id = 2,
                Name = "City2"
            }
        };

        var dbPromotionEntities = new List<PromotionEntity>
        {
            new()
            {
                Percentage = 20,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "1"
            },
            new()
            {
                Percentage = 30,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "2"
            }
        };

        var context = CreateDatabase(
            dbProductEntities,
            dbProductAvailabilityEntities,
            dbStoreEntities,
            dbPriceEntryEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbCompanyEntities,
            dbCityEntities,
            dbPromotionEntities);
        
        var productRepo = new ProductRepository(context);
        
        // Act
        await productRepo.SetProductAvailabilityAsync("1", 1, false);
        
        var expectedAvailability = new List<ProductAvailabilityEntity>
        {
            new()
            {
                ProductId = "1",
                StoreId = 1,
                IsAvailable = false,
                LastChecked = It.IsAny<DateTime>()
            }
        };
        
        // Assert
        context.ProductAvailability.Should().BeEquivalentTo(expectedAvailability, x => x.Excluding(y => y.LastChecked));
    }

    [Fact]
    public async Task UpdateProductAsync_ReturnsObjectAsync()
    {
                        // Arrange
        var dbProductEntities = new List<ProductEntity>
        {
            new()
            {
                Id = "1",
                Name = "Banana",
                BrandId = 1,
                CategoryId = 1,
                ImageUrl = "1",
                Quantity = 1,
                Rating = 1.0,
                Unit = "unidades",
                Views = 1
            },
            new()
            {
                Id = "2",
                Name = "Apple",
                BrandId = 2,
                CategoryId = 2,
                ImageUrl = "2",
                Quantity = 2,
                Rating = 2.0,
                Unit = "unidades",
                Views = 2
            }
        };

        var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
        {
            new()
            {
                ProductId = "1",
                StoreId = 1,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                ProductId = "2",
                StoreId = 2,
                IsAvailable = false,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        var dbStoreEntities = new List<StoreEntity>
        {
            new()
            {
                Id = 1,
                Name = "Store1",
                CompanyId = 1,
                CityId = 1,
                Address = "Address1",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new()
            {
                Id = 2,
                Name = "Store2",
                CompanyId = 2,
                CityId = 2,
                Address = "Address2",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302")
            }
        };

        var dbPriceEntryEntities = new List<PriceEntryEntity>
        {
            new()
            {
                Id = "1",
                ProductId = "1",
                StoreId = 1,
                Price = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = "2",
                ProductId = "2",
                StoreId = 2,
                Price = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
        };

        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand1"
            },
            new()
            {
                Id = 2,
                Name = "Brand2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category1"
            },
            new()
            {
                Id = 2,
                Name = "Category2"
            }
        };

        var dbCompanyEntities = new List<CompanyEntity>
        {
            new()
            {
                Id = 1,
                Name = "Company1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "1",
            },
            new()
            {
                Id = 2,
                Name = "Company2",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "2",
            }
        };

        var dbCityEntities = new List<CityEntity>
        {
            new()
            {
                Id = 1,
                Name = "City1"
            },
            new()
            {
                Id = 2,
                Name = "City2"
            }
        };

        var dbPromotionEntities = new List<PromotionEntity>
        {
            new()
            {
                Percentage = 20,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "1"
            },
            new()
            {
                Percentage = 30,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "2"
            }
        };

        var context = CreateDatabase(
            dbProductEntities,
            dbProductAvailabilityEntities,
            dbStoreEntities,
            dbPriceEntryEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbCompanyEntities,
            dbCityEntities,
            dbPromotionEntities);
        
        var productRepo = new ProductRepository(context);
        
        // Act
        await productRepo.UpdateProductAsync(
            "1",
            "Manga",
            "newImage",
            1,
            "unidades",
            1,
            1
        );

        var expectedProduct = new ProductEntity
        {
            Id = "1",
            Name = "Manga",
            BrandId = 1,
            CategoryId = 1,
            ImageUrl = "newImage",
            Quantity = 1,
            Rating = 1.0,
            Unit = "unidades",
            Views = 1
        };
        
        // Assert
        context.Product.Should().ContainEquivalentOf(expectedProduct);
    }

    [Fact]
    public async Task UpdateProductAsync_WhenProductDoesNotExist_ReturnsNullAsync()
    {
        var context = CreateDatabase(
            new List<ProductEntity>(),
            new List<ProductAvailabilityEntity>(),
            new List<StoreEntity>(),
            new List<PriceEntryEntity>(),
            new List<BrandEntity>(),
            new List<CategoryEntity>(),
            new List<CompanyEntity>(),
            new List<CityEntity>(),
            new List<PromotionEntity>());
        
        var productRepo = new ProductRepository(context);
        
        // Act
        var actualProduct = await productRepo.UpdateProductAsync(
            "1",
            "Manga",
            "newImage",
            1,
            "unidades",
            1,
            1
        );
        
        // Assert
        actualProduct.Should().BeNull();
    }



    [Fact]
    public async Task RemoveProductAsync_WhenProductDoesNotExists_ReturnsObjectAsync()
    {
        var context = CreateDatabase(
            new List<ProductEntity>(),
            new List<ProductAvailabilityEntity>(),
            new List<StoreEntity>(),
            new List<PriceEntryEntity>(),
            new List<BrandEntity>(),
            new List<CategoryEntity>(),
            new List<CompanyEntity>(),
            new List<CityEntity>(),
            new List<PromotionEntity>());
        
        var productRepo = new ProductRepository(context);
        
        // Act
        var actualProduct = await productRepo.RemoveProductAsync("1");
        
        // Assert
        actualProduct.Should().BeNull();
    }

    [Fact]
    public async Task RemoveProductAsync_WhenProductExists_ReturnsObjectAsync()
    {
                        // Arrange
        var dbProductEntities = new List<ProductEntity>
        {
            new()
            {
                Id = "1",
                Name = "Banana",
                BrandId = 1,
                CategoryId = 1,
                ImageUrl = "1",
                Quantity = 1,
                Rating = 1.0,
                Unit = "unidades",
                Views = 1
            },
            new()
            {
                Id = "2",
                Name = "Apple",
                BrandId = 2,
                CategoryId = 2,
                ImageUrl = "2",
                Quantity = 2,
                Rating = 2.0,
                Unit = "unidades",
                Views = 2
            }
        };

        var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
        {
            new()
            {
                ProductId = "1",
                StoreId = 1,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                ProductId = "2",
                StoreId = 2,
                IsAvailable = false,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };

        var dbStoreEntities = new List<StoreEntity>
        {
            new()
            {
                Id = 1,
                Name = "Store1",
                CompanyId = 1,
                CityId = 1,
                Address = "Address1",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new()
            {
                Id = 2,
                Name = "Store2",
                CompanyId = 2,
                CityId = 2,
                Address = "Address2",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302")
            }
        };

        var dbPriceEntryEntities = new List<PriceEntryEntity>
        {
            new()
            {
                Id = "1",
                ProductId = "1",
                StoreId = 1,
                Price = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = "2",
                ProductId = "2",
                StoreId = 2,
                Price = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
        };

        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand1"
            },
            new()
            {
                Id = 2,
                Name = "Brand2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category1"
            },
            new()
            {
                Id = 2,
                Name = "Category2"
            }
        };

        var dbCompanyEntities = new List<CompanyEntity>
        {
            new()
            {
                Id = 1,
                Name = "Company1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "1",
            },
            new()
            {
                Id = 2,
                Name = "Company2",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "2",
            }
        };

        var dbCityEntities = new List<CityEntity>
        {
            new()
            {
                Id = 1,
                Name = "City1"
            },
            new()
            {
                Id = 2,
                Name = "City2"
            }
        };

        var dbPromotionEntities = new List<PromotionEntity>
        {
            new()
            {
                Percentage = 20,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "1"
            },
            new()
            {
                Percentage = 30,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "2"
            }
        };

        var context = CreateDatabase(
            dbProductEntities,
            dbProductAvailabilityEntities,
            dbStoreEntities,
            dbPriceEntryEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbCompanyEntities,
            dbCityEntities,
            dbPromotionEntities);
        
        var productRepo = new ProductRepository(context);
        
        // Act
        var removedProductAsync = await productRepo.RemoveProductAsync("1");
        
        var expectedRemovedProduct = new Product(
            "1",
            "Banana",
            "1",
            1,
            ProductUnit.Units,
            1,
            1.0,
            new Brand(1, "Brand1"),
            new Category(1, "Category1")
        );

        var expectedRemovedEntityProduct = new ProductEntity()
        {
            Id = "1",
            Name = "Banana",
            BrandId = 1,
            CategoryId = 1,
            ImageUrl = "1",
            Quantity = 1,
            Rating = 1.0,
            Unit = "unidades",
            Views = 1
        };
            
        // Assert
        removedProductAsync.Should().BeEquivalentTo(expectedRemovedProduct);
        context.Product.Should().NotContainEquivalentOf(expectedRemovedProduct);
    }

    private static MarketTrackerDataContext CreateDatabase(
        IEnumerable<ProductEntity> productEntities,
        IEnumerable<ProductAvailabilityEntity> productAvailabilityEntities,
        IEnumerable<StoreEntity> storeEntities,
        IEnumerable<PriceEntryEntity> priceEntryEntities,
        IEnumerable<BrandEntity> brandEntities,
        IEnumerable<CategoryEntity> categoryEntities,
        IEnumerable<CompanyEntity> companyEntities,
        IEnumerable<CityEntity> cityEntities,
        IEnumerable<PromotionEntity> promotionEntities)
    {
        DbContextOptions<MarketTrackerDataContext> options =
            new DbContextOptionsBuilder<MarketTrackerDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        var databaseContext = new MarketTrackerDataContext(options);
        
        databaseContext.Product.AddRange(productEntities);
        databaseContext.ProductAvailability.AddRange(productAvailabilityEntities);
        databaseContext.Store.AddRange(storeEntities);
        databaseContext.PriceEntry.AddRange(priceEntryEntities);
        databaseContext.Brand.AddRange(brandEntities);
        databaseContext.Category.AddRange(categoryEntities);
        databaseContext.Company.AddRange(companyEntities);
        databaseContext.City.AddRange(cityEntities);
        databaseContext.Promotion.AddRange(promotionEntities);
        
        databaseContext.SaveChanges();
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }
}

