package pt.isel.markettracker.http.service.operations.alert

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.domain.model.market.price.PriceAlertId
import pt.isel.markettracker.http.service.MarketTrackerService

private const val alertsPath = "/alerts"
private fun buildAlertByIdPath(alertId: String) = "$alertsPath/$alertId"

class AlertService(
    override val httpClient: OkHttpClient,
    override val gson: Gson
) : IAlertService, MarketTrackerService() {
    override suspend fun getAlerts(): List<PriceAlert> {
        return requestHandler<CollectionOutputModel<PriceAlert>>(
            path = alertsPath,
            method = HttpMethod.GET
        ).items
    }

    override suspend fun createAlert(
        productId: String,
        storeId: Int,
        priceThreshold: Int
    ): PriceAlertId {
        return requestHandler<PriceAlertId>(
            path = alertsPath,
            method = HttpMethod.POST,
            body = mapOf(
                "productId" to productId,
                "storeId" to storeId,
                "priceThreshold" to priceThreshold
            )
        )
    }

    override suspend fun deleteAlert(alertId: String) {
        requestHandler<Unit>(
            path = buildAlertByIdPath(alertId),
            method = HttpMethod.DELETE
        )
    }
}