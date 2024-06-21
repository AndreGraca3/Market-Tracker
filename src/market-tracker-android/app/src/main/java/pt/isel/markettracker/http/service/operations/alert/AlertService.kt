package pt.isel.markettracker.http.service.operations.alert

import com.google.gson.Gson
import okhttp3.OkHttpClient
import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.http.models.Alert.PriceAlertCreationInputModel
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel
import pt.isel.markettracker.http.service.MarketTrackerService

private const val alertsPath = "/alerts"

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

    override suspend fun createAlert(priceAlertInput: PriceAlertCreationInputModel): StringIdOutputModel {
        TODO("Not yet implemented")
    }

    override suspend fun deleteAlert() {
        TODO("Not yet implemented")
    }
}