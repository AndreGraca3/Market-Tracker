package pt.isel.markettracker.domain.product

import pt.isel.markettracker.domain.City

data class ProductInfo(
    val id: String,
    val name: String,
    val imageUrl: String,
    val brand: String,
    val category: String
)

data class StoreInfo(
    val id: Int,
    val name: String,
    val address: String,
    val city: City?,
    val isOnline: Boolean,
    val company: CompanyInfo
)

data class CompanyInfo(
    val id: Int,
    val name: String,
    val logoUrl: String
)