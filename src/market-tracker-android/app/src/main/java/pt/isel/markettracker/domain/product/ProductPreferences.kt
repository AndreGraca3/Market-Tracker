package pt.isel.markettracker.domain.product

import pt.isel.markettracker.domain.price.PriceAlert

data class ProductPreferences(val id: String, val priceAlert: PriceAlert?, val isFavorite: Boolean)
