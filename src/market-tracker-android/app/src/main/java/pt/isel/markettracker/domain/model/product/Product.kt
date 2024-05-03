package pt.isel.markettracker.domain.model.product

import pt.isel.markettracker.domain.model.brand.Brand
import pt.isel.markettracker.domain.model.category.Category

data class Product(
    val id: String,
    val name: String,
    val imageUrl: String,
    val quantity: Int,
    val unit: String,
    val rating: Double,
    val brand: Brand,
    val category: Category
)