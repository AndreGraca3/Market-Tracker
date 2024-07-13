using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repository.Market.Inventory;

public class ProductFeedbackRepositoryTest
{
    [Fact]
    public async Task GetReviewsByProductIdAsync_ReturnsListAsync()
    {
        // Arrange
        var dbProductReviewEntities = new List<ProductReviewEntity>()
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Rating = 1,
                ProductId = "1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                Text = "Review 1"
            }
        };
        
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
        
        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand 1"
            },
            new()
            {
                Id = 2,
                Name = "Brand 2"
            }
        };
        
        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category 1"
            },
            new()
            {
                Id = 2,
                Name = "Category 2"
            }
        };

        var dbClientEntities = new List<ClientEntity>
        {
            new()
            {
                UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Avatar = "Avatar1",
                Username = "User1"
            }
        };
        
        var dbProductFavoriteEntities = new List<ProductFavouriteEntity>
        {
            new()
            {
                ProductId = "1",
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            }
        };
        
        var dataContext = CreateDatabase(
            dbProductReviewEntities,
            dbProductEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbClientEntities,
            dbProductFavoriteEntities
        );
        
        var productFeedbackRepository = new ProductFeedbackRepository(dataContext);
        
        // Act
        var actualReview = await productFeedbackRepository.GetReviewsByProductIdAsync("1", 0, 5);
        
        // Assert
        var expectedPaginatedReview = new PaginatedResult<ProductReview>(
            new List<ProductReview>
            {
                new(
                    new ReviewId(1),
                    new ClientItem(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "User1", "Avatar1"),
                    "1",
                    1,
                    "Review 1",
                    new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
                    )
            },
            1,
            0,
            5
            );

        expectedPaginatedReview.Should().BeEquivalentTo(expectedPaginatedReview);
    }

    [Fact]
    public async Task GetReviewByIdAsync_ReturnsReviewAsync()
    {
                // Arrange
        var dbProductReviewEntities = new List<ProductReviewEntity>()
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Rating = 1,
                ProductId = "1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                Text = "Review 1"
            }
        };
        
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
        
        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand 1"
            },
            new()
            {
                Id = 2,
                Name = "Brand 2"
            }
        };
        
        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category 1"
            },
            new()
            {
                Id = 2,
                Name = "Category 2"
            }
        };

        var dbClientEntities = new List<ClientEntity>
        {
            new()
            {
                UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Avatar = "Avatar1",
                Username = "User1"
            }
        };
        
        var dbProductFavoriteEntities = new List<ProductFavouriteEntity>
        {
            new()
            {
                ProductId = "1",
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            }
        };
        
        var dataContext = CreateDatabase(
            dbProductReviewEntities,
            dbProductEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbClientEntities,
            dbProductFavoriteEntities
        );
        
        var productFeedbackRepository = new ProductFeedbackRepository(dataContext);
        
        // Act
        var actualReview = await productFeedbackRepository.GetReviewByIdAsync(1);
        
        // Assert
        var expectedReview = new ProductReview(
            new ReviewId(1),
            new ClientItem(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "User1", "Avatar1"),
            "1",
            1,
            "Review 1",
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );
        
        actualReview.Should().BeEquivalentTo(expectedReview);
    }

    [Fact]
    public async Task AddReviewAsync_ReturnsReviewIdAsync()
    {
        // Arrange
        var dbProductReviewEntities = new List<ProductReviewEntity>()
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Rating = 1,
                ProductId = "1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                Text = "Review 1"
            }
        };

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

        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand 1"
            },
            new()
            {
                Id = 2,
                Name = "Brand 2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category 1"
            },
            new()
            {
                Id = 2,
                Name = "Category 2"
            }
        };

        var dbClientEntities = new List<ClientEntity>
        {
            new()
            {
                UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Avatar = "Avatar1",
                Username = "User1"
            }
        };

        var dbProductFavoriteEntities = new List<ProductFavouriteEntity>
        {
            new()
            {
                ProductId = "1",
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            }
        };

        var dataContext = CreateDatabase(
            dbProductReviewEntities,
            dbProductEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbClientEntities,
            dbProductFavoriteEntities
        );
        
        var productFeedbackRepository = new ProductFeedbackRepository(dataContext);
        
        // Act
        var actualReviewId = await productFeedbackRepository.AddReviewAsync(
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
            "1",
            1,
            "Review 1"
        );
        
        // Assert
        var expectedReviewId = new ReviewId(2);
        actualReviewId.Should().BeEquivalentTo(expectedReviewId);

        var expectedProductReviewEntity = new ProductReviewEntity()
        {
            Id = 2,
            ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
            Rating = 1,
            ProductId = "1",
            CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            Text = "Review 1"
        };

        dataContext.ProductReview.Should().ContainEquivalentOf(expectedProductReviewEntity, x => x.Excluding(y => y.CreatedAt));
    }

    [Fact]
    public async Task UpdateReviewAsync_ReturnsReviewAsync()
    {
        // Arrange
        var dbProductReviewEntities = new List<ProductReviewEntity>()
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Rating = 1,
                ProductId = "1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                Text = "Review 1"
            }
        };

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

        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand 1"
            },
            new()
            {
                Id = 2,
                Name = "Brand 2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category 1"
            },
            new()
            {
                Id = 2,
                Name = "Category 2"
            }
        };

        var dbClientEntities = new List<ClientEntity>
        {
            new()
            {
                UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Avatar = "Avatar1",
                Username = "User1"
            }
        };

        var dbProductFavoriteEntities = new List<ProductFavouriteEntity>
        {
            new()
            {
                ProductId = "1",
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            }
        };

        var dataContext = CreateDatabase(
            dbProductReviewEntities,
            dbProductEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbClientEntities,
            dbProductFavoriteEntities
        );

        var productFeedbackRepository = new ProductFeedbackRepository(dataContext);

        // Act
        var actualReview = await productFeedbackRepository.UpdateReviewAsync(
            1,
            2,
            "Review 2"
        );

        // Assert
        var expectedReview = new ProductReview(
            new ReviewId(1),
            new ClientItem(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "User1", "Avatar1"),
            "1",
            2,
            "Review 2",
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );

        actualReview.Should().BeEquivalentTo(expectedReview);
    }

    [Fact]
    public async Task RemoveReviewAsync_ReturnsReviewAsync()
    {
        // Arrange
        var dbProductReviewEntities = new List<ProductReviewEntity>()
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Rating = 1,
                ProductId = "1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                Text = "Review 1"
            }
        };

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

        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand 1"
            },
            new()
            {
                Id = 2,
                Name = "Brand 2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category 1"
            },
            new()
            {
                Id = 2,
                Name = "Category 2"
            }
        };

        var dbClientEntities = new List<ClientEntity>
        {
            new()
            {
                UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Avatar = "Avatar1",
                Username = "User1"
            }
        };

        var dbProductFavoriteEntities = new List<ProductFavouriteEntity>
        {
            new()
            {
                ProductId = "1",
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            }
        };

        var dataContext = CreateDatabase(
            dbProductReviewEntities,
            dbProductEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbClientEntities,
            dbProductFavoriteEntities
        );
        
        var productFeedbackRepository = new ProductFeedbackRepository(dataContext);
        
        // Act
        var actualReview = await productFeedbackRepository.RemoveReviewAsync(1);
        
        // Assert
        var expectedReview = new ProductReview(
            new ReviewId(1),
            new ClientItem(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "User1", "Avatar1"),
            "1",
            1,
            "Review 1",
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );

        var removedProductReviewEntity = new ProductReviewEntity()
        {
            Id = 1,
            ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
            Rating = 1,
            ProductId = "1",
            CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            Text = "Review 1"
        };
        
        actualReview.Should().BeEquivalentTo(expectedReview);
        dataContext.ProductReview.Should().NotContainEquivalentOf(removedProductReviewEntity, x => x.Excluding(y => y.CreatedAt));
    }

    [Fact]
    public async Task GetFavouriteProductsAsync_ReturnsListAsync()
    {
        // Arrange
        var dbProductReviewEntities = new List<ProductReviewEntity>()
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Rating = 1,
                ProductId = "1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                Text = "Review 1"
            }
        };

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

        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand 1"
            },
            new()
            {
                Id = 2,
                Name = "Brand 2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category 1"
            },
            new()
            {
                Id = 2,
                Name = "Category 2"
            }
        };

        var dbClientEntities = new List<ClientEntity>
        {
            new()
            {
                UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Avatar = "Avatar1",
                Username = "User1"
            }
        };

        var dbProductFavoriteEntities = new List<ProductFavouriteEntity>
        {
            new()
            {
                ProductId = "1",
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            }
        };

        var dataContext = CreateDatabase(
            dbProductReviewEntities,
            dbProductEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbClientEntities,
            dbProductFavoriteEntities
        );
        
        var productFeedbackRepository = new ProductFeedbackRepository(dataContext);
        
        // Act
        var actualFavouriteProducts = await productFeedbackRepository.GetFavouriteProductsAsync(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
        
        // Assert
        var expectedFavouriteProducts = new List<ProductItem>
        {
            new(
                new ProductId("1"),
                "Banana",
                "1",
                "Brand 1"
            )
        };
        
        actualFavouriteProducts.Should().BeEquivalentTo(expectedFavouriteProducts);
    }

    // [Fact]
    // public async Task UpdateProductFavouriteAsync_ReturnsBoolAsync()
    // {
    //             // Arrange
    //     var dbProductReviewEntities = new List<ProductReviewEntity>()
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
    //             Rating = 1,
    //             ProductId = "1",
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             Text = "Review 1"
    //         }
    //     };
    //     
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
    //             Unit = "unidades",
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
    //     var dbBrandEntities = new List<BrandEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Brand 1"
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Brand 2"
    //         }
    //     };
    //     
    //     var dbCategoryEntities = new List<CategoryEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Category 1"
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Category 2"
    //         }
    //     };
    //
    //     var dbClientEntities = new List<ClientEntity>
    //     {
    //         new()
    //         {
    //             UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
    //             Avatar = "Avatar1",
    //             Username = "User1"
    //         }
    //     };
    //     
    //     var dbProductFavoriteEntities = new List<ProductFavouriteEntity>
    //     {
    //         new()
    //         {
    //             ProductId = "1",
    //             ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
    //         }
    //     };
    //     
    //     var dataContext = CreateDatabase(
    //         dbProductReviewEntities,
    //         dbProductEntities,
    //         dbBrandEntities,
    //         dbCategoryEntities,
    //         dbClientEntities,
    //         dbProductFavoriteEntities
    //     );
    //     
    //     var productFeedbackRepository = new ProductFeedbackRepository(dataContext);
    //     
    //     // Act
    //     var actualIsFavourite = await productFeedbackRepository.UpdateProductFavouriteAsync(
    //         Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
    //         "1",
    //         true
    //     );
    //     
    //     // Assert
    //     actualIsFavourite.Should().BeFalse();
    // }

    [Fact]
    public async Task GetProductPreferencesAsync_ReturnsObjectAsync()
    {
                // Arrange
        var dbProductReviewEntities = new List<ProductReviewEntity>()
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Rating = 1,
                ProductId = "1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                Text = "Review 1"
            }
        };
        
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
        
        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand 1"
            },
            new()
            {
                Id = 2,
                Name = "Brand 2"
            }
        };
        
        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category 1"
            },
            new()
            {
                Id = 2,
                Name = "Category 2"
            }
        };

        var dbClientEntities = new List<ClientEntity>
        {
            new()
            {
                UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Avatar = "Avatar1",
                Username = "User1"
            }
        };
        
        var dbProductFavoriteEntities = new List<ProductFavouriteEntity>
        {
            new()
            {
                ProductId = "1",
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            }
        };
        
        var dataContext = CreateDatabase(
            dbProductReviewEntities,
            dbProductEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbClientEntities,
            dbProductFavoriteEntities
        );
        
        var productFeedbackRepository = new ProductFeedbackRepository(dataContext);
        
        // Act
        var actualProductPreferences = await productFeedbackRepository.GetProductPreferencesAsync(
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
            "1"
        );
        
        // Assert
        var expectedProductPreferences = new ProductPreferences(
            true,
            new ProductReview(
                new ReviewId(1),
                new ClientItem(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "User1", "Avatar1"),
                "1",
                1,
                "Review 1",
                new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            )
        );
        
        actualProductPreferences.Should().BeEquivalentTo(expectedProductPreferences);
    }

    [Fact]
    public async Task GetProductStatsByIdAsync_ReturnsObjectAsync()
    {
                // Arrange
        var dbProductReviewEntities = new List<ProductReviewEntity>()
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Rating = 1,
                ProductId = "1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                Text = "Review 1"
            }
        };
        
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
        
        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand 1"
            },
            new()
            {
                Id = 2,
                Name = "Brand 2"
            }
        };
        
        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category 1"
            },
            new()
            {
                Id = 2,
                Name = "Category 2"
            }
        };

        var dbClientEntities = new List<ClientEntity>
        {
            new()
            {
                UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Avatar = "Avatar1",
                Username = "User1"
            }
        };
        
        var dbProductFavoriteEntities = new List<ProductFavouriteEntity>
        {
            new()
            {
                ProductId = "1",
                ClientId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            }
        };

        var productStatsCountsEntities = new ProductStatsCountsEntity()
        {
            ProductId = "1",
            Favourites = 1,
            Ratings = 1
        };
        
        var dataContext = CreateDatabase(
            dbProductReviewEntities,
            dbProductEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbClientEntities,
            dbProductFavoriteEntities
        );
        
        dataContext.ProductStatsCounts.AddRange(productStatsCountsEntities);
        await dataContext.SaveChangesAsync();
        
        var productFeedbackRepository = new ProductFeedbackRepository(dataContext);
        
        // Act
        var actualProductStats = await productFeedbackRepository.GetProductStatsByIdAsync("1");
        
        // Assert
        var expectedProductStats = new ProductStats(
                "1",
                new ProductStatsCounts(1, 1),
                1.0);
        
        actualProductStats.Should().BeEquivalentTo(expectedProductStats);
    }

    private static MarketTrackerDataContext CreateDatabase(
        IEnumerable<ProductReviewEntity> productReviewEntities,
        IEnumerable<ProductEntity> productEntities,
        IEnumerable<BrandEntity> brandEntities,
        IEnumerable<CategoryEntity> categoryEntities,
        IEnumerable<ClientEntity> clientEntities,
        IEnumerable<ProductFavouriteEntity> productFavoriteEntities)
    {
        DbContextOptions<MarketTrackerDataContext> options =
            new DbContextOptionsBuilder<MarketTrackerDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        var databaseContext = new MarketTrackerDataContext(options);
        
        databaseContext.ProductReview.AddRange(productReviewEntities);
        databaseContext.Product.AddRange(productEntities);
        databaseContext.Brand.AddRange(brandEntities);
        databaseContext.Category.AddRange(categoryEntities);
        databaseContext.Client.AddRange(clientEntities);
        databaseContext.ProductFavorite.AddRange(productFavoriteEntities);
        
        databaseContext.SaveChanges();
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }
}