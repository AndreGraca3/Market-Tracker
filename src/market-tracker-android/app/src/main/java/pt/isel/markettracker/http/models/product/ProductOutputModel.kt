package pt.isel.markettracker.http.models.product

import pt.isel.markettracker.domain.model.category.Category
import pt.isel.markettracker.domain.model.brand.Brand
import pt.isel.markettracker.domain.model.product.Product

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
        unit,
        rating,
        brand.name,
        category.name
    )
}