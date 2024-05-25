// using FluentAssertions;
// using market_tracker_webapi.Application.Domain;
// using market_tracker_webapi.Application.Repository.Operations.Product;
// using market_tracker_webapi.Infrastructure.PostgreSQLTables;
// using Xunit.Abstractions;
//
// namespace market_tracker_webapi_test.Application.Repository;
//
// public class ProductRepositoryTest
// {
//     private readonly ITestOutputHelper _testOutputHelper;
//
//     private static readonly List<ProductEntity> _dummyProducts =
//     [
//         new ProductEntity
//         {
//             Id = 1,
//             Name = "Filipinos",
//             ImageUrl = "dummy_image_url",
//             Quantity = 1,
//             Unit = "unidades",
//             BrandId = 1,
//             CategoryId = 12
//         },
//         new ProductEntity
//         {
//             Id = 2,
//             Name = "Banana",
//             ImageUrl = "dummy_image_url",
//             Quantity = 1,
//             Unit = "unidades",
//             BrandId = 2,
//             CategoryId = 12
//         },
//         new ProductEntity
//         {
//             Id = 3,
//             Name = "Gomas",
//             ImageUrl = "dummy_image_url",
//             Quantity = 1,
//             Unit = "unidades",
//             BrandId = 3,
//             CategoryId = 5
//         }
//     ];
//
//     private readonly List<ProductReviewEntity> _dummyReviews =
//     [
//         new ProductReviewEntity
//         {
//             ClientId = Guid.NewGuid(),
//             ProductId = _dummyProducts[0].Id,
//             Rating = 5,
//             Text = "Great product",
//             CreatedAt = DateTime.Now - TimeSpan.FromDays(3)
//         },
//         new ProductReviewEntity
//         {
//             ClientId = Guid.NewGuid(),
//             ProductId = _dummyProducts[0].Id,
//             Rating = 4,
//             Text = "Good product",
//             CreatedAt = DateTime.Now - TimeSpan.FromDays(2)
//         },
//         new ProductReviewEntity
//         {
//             ClientId = Guid.NewGuid(),
//             ProductId = _dummyProducts[0].Id,
//             Rating = 2,
//             Text = "Bad product",
//             CreatedAt = DateTime.Now - TimeSpan.FromDays(1)
//         },
//         new ProductReviewEntity
//         {
//             ClientId = Guid.NewGuid(),
//             ProductId = _dummyProducts[1].Id,
//             Rating = 3,
//             Text = "Ok product",
//             CreatedAt = DateTime.Now
//         }
//     ];
//
//     public ProductRepositoryTest(ITestOutputHelper testOutputHelper)
//     {
//         _testOutputHelper = testOutputHelper;
//     }
//
//     [Fact]
//     public async Task GetProductsAsync_ReturnsListAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualProducts = await productRepo.GetProductsAsync();
//
//         // Assert
//         actualProducts.Should().BeEquivalentTo(_dummyProducts);
//     }
//
//     [Fact]
//     public async Task GetProductByIdAsync_ReturnsObjectAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualProduct = await productRepo.GetProductByIdAsync(_dummyProducts[0].Id);
//
//         // Assert
//         actualProduct.Should().BeEquivalentTo(_dummyProducts[0]);
//     }
//
//     [Fact]
//     public async Task GetProductByIdAsync_ReturnsNullAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualProduct = await productRepo.GetProductByIdAsync(4);
//
//         // Assert
//         actualProduct.Should().BeNull();
//     }
//
//     [Fact]
//     public async Task AddProductAsync_ReturnsObjectAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts.Take(2).ToList());
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualProduct = await productRepo.AddProductAsync(
//             _dummyProducts[2].Id,
//             _dummyProducts[2].Name,
//             _dummyProducts[2].ImageUrl,
//             _dummyProducts[2].Quantity,
//             _dummyProducts[2].Unit,
//             _dummyProducts[2].BrandId,
//             _dummyProducts[2].CategoryId
//         );
//
//         // Assert
//         actualProduct.Should().Be(_dummyProducts[2].Id);
//     }
//
//     [Fact]
//     public async Task UpdateProductAsync_ReturnsObjectAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts);
//         var productRepo = new ProductRepository(context);
//         var expectedProduct = new Product(
//             _dummyProducts[0].Id,
//             "new_name",
//             "new_image_url",
//             2,
//             "kg",
//             2,
//             2,
//             0,
//             1
//         );
//
//         // Act
//         var actualProduct = await productRepo.UpdateProductAsync(
//             _dummyProducts[0].Id,
//             expectedProduct.Name,
//             expectedProduct.ImageUrl,
//             expectedProduct.Quantity,
//             expectedProduct.Unit,
//             expectedProduct.BrandId,
//             expectedProduct.CategoryId
//         );
//
//         // Assert
//         Assert.NotNull(actualProduct);
//         actualProduct.Name.Should().Be(expectedProduct.Name);
//         actualProduct.ImageUrl.Should().Be(expectedProduct.ImageUrl);
//         actualProduct.Quantity.Should().Be(expectedProduct.Quantity);
//         actualProduct.Unit.Should().Be(expectedProduct.Unit);
//         actualProduct.BrandId.Should().Be(expectedProduct.BrandId);
//         actualProduct.CategoryId.Should().Be(expectedProduct.CategoryId);
//     }
//
//     [Fact]
//     public async Task UpdateProductImageAsync_ReturnsUpdatedProductAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts);
//         var productRepo = new ProductRepository(context);
//         var expectedProduct = new Product(
//             _dummyProducts[0].Id,
//             _dummyProducts[0].Name,
//             "new_image_url",
//             _dummyProducts[0].Quantity,
//             _dummyProducts[0].Unit,
//             _dummyProducts[0].Views,
//             _dummyProducts[0].Rating,
//             _dummyProducts[0].BrandId,
//             _dummyProducts[0].CategoryId
//         );
//
//         // Act
//         var actualProduct = await productRepo.UpdateProductAsync(
//             _dummyProducts[0].Id,
//             null,
//             imageUrl: expectedProduct.ImageUrl
//         );
//
//         // Assert
//         Assert.NotNull(actualProduct);
//         actualProduct.Should().BeEquivalentTo(expectedProduct);
//     }
//
//     [Fact]
//     public async Task UpdateProductAsync_ReturnsNullAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualProduct = await productRepo.UpdateProductAsync(
//             4,
//             "new_name",
//             "new_image_url",
//             2,
//             "kg",
//             2,
//             2
//         );
//
//         // Assert
//         actualProduct.Should().BeNull();
//     }
//
//     [Fact]
//     public async Task RemoveProductAsync_ReturnsObjectAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualProduct = await productRepo.RemoveProductAsync(_dummyProducts[0].Id);
//
//         // Assert
//         actualProduct.Should().BeEquivalentTo(_dummyProducts[0]);
//     }
//
//     [Fact]
//     public async Task RemoveProductAsync_ReturnsNullAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualProduct = await productRepo.RemoveProductAsync(4);
//
//         // Assert
//         actualProduct.Should().BeNull();
//     }
//
//     [Fact]
//     public async Task GetReviewsByProductIdAsync_ReturnsListWithClientReviewFirstAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts, _dummyReviews);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualReviews = await productRepo.GetReviewsByProductIdAsync(
//             _dummyReviews[1].ClientId,
//             _dummyProducts[0].Id
//         );
//
//         // Assert
//         actualReviews.First().ClientId.Should().Be(_dummyReviews[1].ClientId); // client's review first
//         actualReviews.Skip(1).First().ClientId.Should().Be(_dummyReviews[2].ClientId); // ordered by date
//         actualReviews.Skip(2).First().ClientId.Should().Be(_dummyReviews[0].ClientId); // ordered by date
//     }
//
//     [Fact]
//     public async Task AddReviewAsync_ReturnsObjectAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts, _dummyReviews.Take(1));
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         await productRepo.AddReviewAsync(
//             _dummyReviews[1].ClientId,
//             _dummyReviews[1].ProductId,
//             5,
//             "Great product"
//         );
//
//         // Assert
//         var actualReview = await context.ProductReview.FindAsync(
//             _dummyReviews[1].ClientId,
//             _dummyReviews[1].ProductId
//         );
//         Assert.NotNull(actualReview);
//         actualReview
//             .Should()
//             .BeEquivalentTo(
//                 new ProductReviewEntity
//                 {
//                     ClientId = _dummyReviews[1].ClientId,
//                     ProductId = _dummyReviews[1].ProductId,
//                     Rating = 5,
//                     Text = "Great product",
//                     CreatedAt = actualReview.CreatedAt
//                 }
//             );
//     }
//
//     [Fact]
//     public async Task UpdateReviewAsync_ReturnsObjectAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts, _dummyReviews);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualReview = await productRepo.UpdateReviewAsync(
//             _dummyReviews[0].ClientId,
//             _dummyReviews[0].ProductId,
//             3,
//             "Great product"
//         );
//
//         // Assert
//         actualReview
//             .Should()
//             .BeEquivalentTo(
//                 new ProductReviewEntity
//                 {
//                     ClientId = _dummyReviews[0].ClientId,
//                     ProductId = _dummyReviews[0].ProductId,
//                     Rating = 3,
//                     Text = "Great product",
//                     CreatedAt = _dummyReviews[0].CreatedAt
//                 }
//             );
//     }
//
//     [Fact]
//     public async Task UpdateReviewAsync_ReturnsNullAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts, _dummyReviews);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualReview = await productRepo.UpdateReviewAsync(
//             Guid.NewGuid(),
//             _dummyReviews[0].ProductId,
//             3,
//             "Great product"
//         );
//
//         // Assert
//         actualReview.Should().BeNull();
//     }
//
//     [Fact]
//     public async Task RemoveReviewAsync_ReturnsObjectAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts, _dummyReviews);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualReview = await productRepo.RemoveReviewAsync(
//             _dummyReviews[0].ClientId,
//             _dummyReviews[0].ProductId
//         );
//
//         // Assert
//         actualReview.Should().BeEquivalentTo(_dummyReviews[0]);
//     }
//
//     [Fact]
//     public async Task RemoveReviewAsync_ReturnsNullAsync()
//     {
//         // Arrange
//         var context = DbHelper.CreateDatabase(_dummyProducts, _dummyReviews);
//         var productRepo = new ProductRepository(context);
//
//         // Act
//         var actualReview = await productRepo.RemoveReviewAsync(
//             Guid.NewGuid(),
//             _dummyReviews[0].ProductId
//         );
//
//         // Assert
//         actualReview.Should().BeNull();
//     }
// }

