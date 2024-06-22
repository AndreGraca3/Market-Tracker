package pt.isel.markettracker.http.service.operations.product

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.domain.model.market.price.ProductPrices
import pt.isel.markettracker.http.service.MarketTrackerService

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
        (if (brandIds.isNotEmpty()) brandIds.joinToString(
            separator = "&brandIds=",
            prefix = "&brandIds="
        ) else "") +
        (if (companyIds.isNotEmpty()) companyIds.joinToString(
            separator = "&companyIds=",
            prefix = "&companyIds="
        ) else "") +
        (if (categoryIds.isNotEmpty()) categoryIds.joinToString(
            separator = "&categoryIds=",
            prefix = "&categoryIds="
        ) else "")

private fun buildProductByIdPath(id: String) = "/products/$id"

private fun buildProductPricesByIdPath(id: String) = "/products/$id/prices"

private fun buildProductReviewsByIdPath(id: String, page: Int, itemsPerPage: Int?) =
    "/products/$id/reviews?page=$page" +
            itemsPerPage?.let { "&itemsPerPage=$it" }.orEmpty()

private fun buildProductStatsByIdPath(id: String) = "/products/$id/stats"

private fun buildProductPreferencesByIdPath(id: String) = "/products/$id/me"

private fun buildAddProductToListPath(listId: String) = "/lists/${listId}/entries"

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
        return requestHandler(
            path = buildProductPreferencesByIdPath(productId),
            method = HttpMethod.GET
        )
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
        comment: String?
    ): ProductReview {
        data class ReviewCreationRequest(
            val rating: Int,
            val comment: String?
        )

        val prefs = requestHandler<ProductPreferences>(
            path = buildProductPreferencesByIdPath(productId),
            method = HttpMethod.PATCH,
            body = mapOf(
                "review" to ReviewCreationRequest(
                    rating = rating,
                    comment = comment
                )
            )
        )

        return prefs.review!!
    }

    override suspend fun deleteProductReview(productId: String) {
        return requestHandler(
            path = buildProductPreferencesByIdPath(productId),
            method = HttpMethod.PATCH,
            body = mapOf(
                "review" to null
            )
        )
    }

    override suspend fun updateFavouriteProduct(productId: String, favourite: Boolean) {
        return requestHandler(
            path = buildProductPreferencesByIdPath(productId),
            method = HttpMethod.PATCH,
            body = mapOf(
                "isFavourite" to favourite
            )
        )
    }

    override suspend fun addProductToList(listId: String, productId: String, storeId: Int) {
        return requestHandler(
            path = buildAddProductToListPath(listId),
            method = HttpMethod.POST,
            body = mapOf( // TODO: check if possible, if not, change to a class
                "productId" to productId,
                "storeId" to storeId,
                "quantity" to 1
            )
        )
    }
}