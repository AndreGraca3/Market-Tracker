package pt.isel.markettracker.http.service.operations.product

import pt.isel.markettracker.domain.product.ProductInfo

interface IProductService {
    suspend fun getProducts(querySearch: String?): List<ProductInfo>

    suspend fun getProductById(id: String): ProductInfo
}