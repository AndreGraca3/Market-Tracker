package pt.isel.markettracker.domain.model.market.inventory.product

import pt.isel.markettracker.domain.model.market.inventory.Brand
import pt.isel.markettracker.domain.model.market.inventory.Category
import pt.isel.markettracker.domain.model.market.inventory.ProductUnit

data class Product(
    val id: String,
    val name: String,
    val imageUrl: String,
    val quantity: Int,
    val unit: String,
    val brand: Brand,
    val category: Category
)