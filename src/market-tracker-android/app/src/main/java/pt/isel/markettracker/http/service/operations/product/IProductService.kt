package pt.isel.markettracker.http.service.operations.product

import pt.isel.markettracker.domain.model.market.inventory.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
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

    suspend fun getProductById(id: String): Product

    suspend fun getProductPrices(id: String): List<CompanyPrices>

    suspend fun getProductStats(id: String): ProductStats

    suspend fun getProductPreferences(id: String): ProductPreferences

    suspend fun getProductAlerts(id: String): List<PriceAlert>
}