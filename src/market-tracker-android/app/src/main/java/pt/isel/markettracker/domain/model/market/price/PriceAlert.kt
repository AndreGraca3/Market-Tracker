package pt.isel.markettracker.domain.model.market.price

data class PriceAlert(val productId: String, val storeId: Int, val priceThreshold: Int)