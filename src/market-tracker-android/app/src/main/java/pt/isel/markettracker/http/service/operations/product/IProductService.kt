package pt.isel.markettracker.http.service.operations.product

import pt.isel.markettracker.domain.model.product.PaginatedProductOffers
import pt.isel.markettracker.domain.model.product.ProductInfo
import pt.isel.markettracker.domain.model.product.ProductPreferences
import pt.isel.markettracker.domain.model.product.ProductStats
import pt.isel.markettracker.http.models.price.CompanyPrices

interface IProductService {
    suspend fun getProducts(
        page: Int,
        itemsPerPage: Int? = null,
        searchQuery: String? = null,
        sortOption: String? = null
    ): PaginatedProductOffers

    suspend fun getProductById(id: String): ProductInfo

    suspend fun getProductPrices(id: String): List<CompanyPrices>

    suspend fun getProductStats(id: String): ProductStats

    suspend fun getProductPreferences(id: String): ProductPreferences
}