package pt.isel.markettracker.http.service.operations.product

import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.price.CompanyPrices
import pt.isel.markettracker.domain.model.market.price.PriceAlert

interface IProductService {
    suspend fun getProducts(
        page: Int,
        itemsPerPage: Int? = null,
        searchQuery: String? = null,
        sortOption: String? = null
    ): PaginatedProductOffers

    suspend fun getProductById(productId: String): Product

    suspend fun getProductPrices(productId: String): List<CompanyPrices>

    suspend fun getProductStats(productId: String): ProductStats

    suspend fun getProductPreferences(productId: String): ProductPreferences

    suspend fun getProductAlerts(productId: String): List<PriceAlert>

    suspend fun getProductReviews(productId: String, page: Int, itemsPerPage: Int? = null): PaginatedResult<ProductReview>

    suspend fun submitProductReview(
        productId: String,
        rating: Int,
        review: String?
    ): ProductReview
}