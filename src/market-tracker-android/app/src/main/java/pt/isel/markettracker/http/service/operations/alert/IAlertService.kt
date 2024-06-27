package pt.isel.markettracker.http.service.operations.alert

import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.domain.model.market.price.PriceAlertId

interface IAlertService {
    suspend fun getAlerts(): List<PriceAlert>

    suspend fun createAlert(
        productId: String,
        storeId: Int,
        priceThreshold: Int
    ): PriceAlertId

    suspend fun deleteAlert(alertId: String)
}