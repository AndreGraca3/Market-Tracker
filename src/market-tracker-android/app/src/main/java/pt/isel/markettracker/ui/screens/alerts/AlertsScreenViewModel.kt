package pt.isel.markettracker.ui.screens.alerts

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.service.operations.alert.IAlertService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.repository.auth.IAuthRepository
import javax.inject.Inject

@HiltViewModel
class AlertsScreenViewModel @Inject constructor(
    private val alertsService: IAlertService,
    private val authRepository: IAuthRepository,
) : ViewModel() {
    private val _alertsFlow: MutableStateFlow<AlertsScreenState> =
        MutableStateFlow(AlertsScreenState.Idle)
    val alerts
        get() = _alertsFlow.asStateFlow()

    fun fetchAlerts() {
        if (_alertsFlow.value !is AlertsScreenState.Idle) return

        _alertsFlow.value = AlertsScreenState.Loading
        viewModelScope.launch {
            runCatchingAPIFailure {
                alertsService.getAlerts()
            }.onSuccess {
                _alertsFlow.value = AlertsScreenState.Loaded(it)
            }.onFailure {
                _alertsFlow.value = AlertsScreenState.Failed(it)
            }
        }
    }

    fun removeFromAlerts(alertId: String) {
        val oldState = _alertsFlow.value
        if (oldState !is AlertsScreenState.Loaded) return

        val oldAlerts = oldState.alerts.toMutableList()

        viewModelScope.launch {
            runCatchingAPIFailure {
                alertsService.deleteAlert(alertId)
            }.onSuccess {
                oldAlerts.removeIf { it.id == alertId }
                authRepository.removeAlert(alertId)
                _alertsFlow.value = AlertsScreenState.Loaded(oldAlerts.toList())
            }.onFailure {
                _alertsFlow.value = AlertsScreenState.Failed(it)
            }
        }
    }
}