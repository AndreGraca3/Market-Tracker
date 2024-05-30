package pt.isel.markettracker.http.service.operations.product

import com.google.gson.Gson
import kotlinx.coroutines.delay
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStatsCounts
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.domain.model.market.price.CompanyPrices
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.domain.model.market.price.ProductPrices
import pt.isel.markettracker.dummy.dummyCompanyPrices
import pt.isel.markettracker.http.service.MarketTrackerService
import java.time.LocalDateTime
private fun buildProductsPath(
    page: Int,
    itemsPerPage: Int?,
    searchQuery: String?,
    brandIds: List<Int>,
    companyIds: List<Int>,
    categoryIds: List<Int>,
    sortOption: String?
) = "/products?page=$page" +
        itemsPerPage?.let { "&itemsPerPage=$it" }.orEmpty() +
        searchQuery?.let { "&name=$it" }.orEmpty() +
        sortOption?.let { "&sortBy=$it" }.orEmpty() +
        (if (brandIds.isNotEmpty()) brandIds.joinToString(separator = "&brandIds=", prefix = "&brandIds=") else "") +
        (if (companyIds.isNotEmpty()) companyIds.joinToString(separator = "&companyIds=", prefix = "&companyIds=") else "") +
        (if (categoryIds.isNotEmpty()) categoryIds.joinToString(separator = "&categoryIds=", prefix = "&categoryIds=") else "")

private fun buildProductByIdPath(id: String) = "/products/$id"

private fun buildProductPricesByIdPath(id: String) = "/products/$id/prices"

private fun buildProductReviewsByIdPath(id: String, page: Int, itemsPerPage: Int?) =
    "/products/$id/reviews?page=$page" +
            itemsPerPage?.let { "&itemsPerPage=$it" }.orEmpty()

private fun buildProductStatsByIdPath(id: String) = "/products/$id/stats"

// Service provides operations related to products.
class ProductService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IProductService, MarketTrackerService() {
    override suspend fun getProducts(
        page: Int,
        itemsPerPage: Int?,
        query: ProductsQuery
    ): PaginatedProductOffers {
        return requestHandler(
            path = buildProductsPath(
                page = page,
                itemsPerPage = itemsPerPage,
                searchQuery = query.searchTerm,
                brandIds = query.filters.brands.mapNotNull { if (it.isSelected) it.id else null },
                companyIds = query.filters.companies.mapNotNull { if (it.isSelected) it.id else null },
                categoryIds = query.filters.categories.mapNotNull { if (it.isSelected) it.id else null },
                sortOption = query.sortOption.name
            ),
            method = HttpMethod.GET
        )
    }

    override suspend fun getProductById(productId: String): Product {
        return requestHandler(
            path = buildProductByIdPath(productId),
            method = HttpMethod.GET
        )
    }

    override suspend fun getProductPrices(productId: String): ProductPrices {
        return requestHandler(
            path = buildProductPricesByIdPath(productId),
            method = HttpMethod.GET
        )
    }

    override suspend fun getProductStats(productId: String): ProductStats {
        return requestHandler(
            path = buildProductStatsByIdPath(productId),
            method = HttpMethod.GET
        )
    }

    override suspend fun getProductPreferences(productId: String): ProductPreferences {
        return ProductPreferences(
            false,
            ProductReview(
                1,
                "1",
                3,
                "Gostei bastante do produto, recomendo e penso comprar mais vezes mas o preço é um pouco alto.",
                LocalDateTime.now(),
                ClientItem(
                    "1",
                    "João",
                    "https://i.imgur.com/fL67hTu.jpeg"
                )
            )
        )
    }

    override suspend fun getProductAlerts(productId: String): List<PriceAlert> {
        return List(10) {
            PriceAlert(
                "1",
                3,
                35
            )
        }
    }

    override suspend fun getProductReviews(
        productId: String,
        page: Int,
        itemsPerPage: Int?
    ): PaginatedResult<ProductReview> {
        return requestHandler(
            path = buildProductReviewsByIdPath(productId, page, itemsPerPage),
            method = HttpMethod.GET
        )
    }

    override suspend fun submitProductReview(
        productId: String,
        rating: Int,
        review: String?
    ): ProductReview {
        return ProductReview(
            1,
            "1",
            rating,
            review,
            LocalDateTime.now(),
            ClientItem(
                "1",
                "João",
                null
            )
        )
    }
}