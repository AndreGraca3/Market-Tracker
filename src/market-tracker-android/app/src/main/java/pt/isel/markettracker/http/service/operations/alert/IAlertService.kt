package pt.isel.markettracker.http.service.operations.alert

import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.http.models.Alert.PriceAlertCreationInputModel
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel

interface IAlertService {
    suspend fun getAlerts(): List<PriceAlert>

    suspend fun createAlert(priceAlertInput: PriceAlertCreationInputModel): StringIdOutputModel

    suspend fun deleteAlert()
}