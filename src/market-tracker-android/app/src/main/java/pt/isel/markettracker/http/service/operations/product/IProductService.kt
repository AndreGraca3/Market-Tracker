package pt.isel.markettracker.http.service.operations.product

import pt.isel.markettracker.domain.product.ProductInfo

interface IProductService {
    suspend fun getProducts(): List<ProductInfo>
}