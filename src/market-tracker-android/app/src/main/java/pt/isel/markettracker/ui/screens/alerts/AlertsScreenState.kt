package pt.isel.markettracker.ui.screens.alerts

import pt.isel.markettracker.domain.model.market.price.PriceAlert

sealed class AlertsScreenState {
    data object Idle : AlertsScreenState()

    data object Loading : AlertsScreenState()

    data class Loaded(
        val alerts: List<PriceAlert>,
    ) : AlertsScreenState()

    data class Failed(val error: Throwable) : AlertsScreenState()
}

fun AlertsScreenState.extractAlerts() =
    when (this) {
        is AlertsScreenState.Loaded -> alerts
        else -> emptyList()
    }