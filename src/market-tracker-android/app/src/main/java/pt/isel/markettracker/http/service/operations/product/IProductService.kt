package pt.isel.markettracker.http.service.operations.product

import pt.isel.markettracker.domain.product.ProductInfo

interface IProductService {
    suspend fun getProducts(query: String?): List<ProductInfo>

    suspend fun getProductById(id: String): ProductInfo
}