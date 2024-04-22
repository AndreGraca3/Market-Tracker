package pt.isel.markettracker.http.service.operations.product

import pt.isel.markettracker.http.models.price.CompanyPrices
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.domain.product.ProductPreferences
import pt.isel.markettracker.domain.product.ProductStats
import pt.isel.markettracker.ui.screens.products.ProductsSortOption

interface IProductService {
    suspend fun getProducts(querySearch: String?, sortOption: String): List<ProductInfo>

    suspend fun getProductById(id: String): ProductInfo

    suspend fun getProductPrices(id: String): List<CompanyPrices>

    suspend fun getProductStats(id: String): ProductStats

    suspend fun getProductPreferences(id: String): ProductPreferences
}