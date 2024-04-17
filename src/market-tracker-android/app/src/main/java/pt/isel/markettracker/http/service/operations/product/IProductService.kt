package pt.isel.markettracker.http.service.operations.product

import pt.isel.markettracker.domain.price.CompanyPrices
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.domain.product.ProductStats

interface IProductService {
    suspend fun getProducts(querySearch: String?): List<ProductInfo>

    suspend fun getProductById(id: String): ProductInfo

    suspend fun getProductPrices(id: String): List<CompanyPrices>

    suspend fun getProductStats(id: String): ProductStats
}