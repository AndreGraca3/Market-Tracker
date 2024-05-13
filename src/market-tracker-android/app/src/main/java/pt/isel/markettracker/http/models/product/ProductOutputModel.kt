package pt.isel.markettracker.http.models.product

import pt.isel.markettracker.domain.model.market.inventory.Category
import pt.isel.markettracker.domain.model.market.inventory.Brand
import pt.isel.markettracker.domain.model.market.inventory.ProductUnit
import pt.isel.markettracker.domain.model.market.inventory.product.Product

data class ProductOutputModel(
    val id: String,
    val name: String,
    val imageUrl: String,
    val quantity: Int,
    val unit: String,
    val rating: Double,
    val brand: Brand,
    val category: Category
) {
    fun toProduct() = Product(
        id,
        name,
        imageUrl,
        quantity,
        ProductUnit.fromTitle(unit),
        rating,
        brand,
        category
    )
}