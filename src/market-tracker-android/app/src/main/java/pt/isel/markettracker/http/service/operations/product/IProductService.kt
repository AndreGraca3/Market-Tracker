package pt.isel.markettracker.http.service.operations.product

import pt.isel.markettracker.domain.PaginatedResult
import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.inventory.product.filter.ProductsQuery
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.domain.model.market.price.ProductPrices

interface IProductService {
    suspend fun getProducts(
        page: Int,
        itemsPerPage: Int? = null,
        query: ProductsQuery
    ): PaginatedProductOffers

    suspend fun getProductById(productId: String): Product

    suspend fun getProductPrices(productId: String): ProductPrices

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