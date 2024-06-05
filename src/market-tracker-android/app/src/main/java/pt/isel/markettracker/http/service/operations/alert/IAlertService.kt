package pt.isel.markettracker.http.service.operations.alert

import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.domain.model.PriceAlertOutputModel
import pt.isel.markettracker.http.models.Alert.PriceAlertCreationInputModel
import pt.isel.markettracker.http.models.identifiers.StringIdOutputModel

interface IAlertService {
    suspend fun getAlerts(): CollectionOutputModel<PriceAlertOutputModel>

    suspend fun createAlert(priceAlertInput: PriceAlertCreationInputModel): StringIdOutputModel

    suspend fun deleteAlert()
}