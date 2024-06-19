package pt.isel.markettracker.domain.model.market.inventory.product

import pt.isel.markettracker.domain.model.market.inventory.Brand
import pt.isel.markettracker.domain.model.market.inventory.Category

data class Product(
    val id: String,
    val name: String,
    val imageUrl: String,
    val quantity: Int,
    val unit: ProductUnit,
    val rating: Double,
    val brand: Brand,
    val category: Category
)