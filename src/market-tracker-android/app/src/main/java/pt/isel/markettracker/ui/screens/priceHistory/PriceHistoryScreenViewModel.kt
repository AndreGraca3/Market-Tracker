package pt.isel.markettracker.ui.screens.priceHistory

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.domain.model.market.price.ProductAlert
import pt.isel.markettracker.domain.model.market.price.StoreItem
import pt.isel.markettracker.http.service.operations.alert.IAlertService
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.ui.screens.product.alert.PriceAlertState
import java.time.LocalDateTime
import javax.inject.Inject

@HiltViewModel
class PriceHistoryScreenViewModel @Inject constructor(
    private val productService: IProductService,
    private val alertService: IAlertService,
    private val authRepository: IAuthRepository,
) : ViewModel() {

    private val _priceHistoryFlow: MutableStateFlow<PriceHistoryScreenState> =
        MutableStateFlow(PriceHistoryScreenState.Idle)
    val priceHistory
        get() = _priceHistoryFlow.asStateFlow()
    
    
    fun fetchPriceHistory(productId: String, storeId: Int) {
        if(_priceHistoryFlow.value is PriceHistoryScreenState.Loading) return

        _priceHistoryFlow.value = PriceHistoryScreenState.Loading

        viewModelScope.launch {
            runCatchingAPIFailure {
                productService.getPriceHistory(productId, storeId)
            }.onSuccess {
                _priceHistoryFlow.value = PriceHistoryScreenState.Loaded(it)
            }.onFailure {
                _priceHistoryFlow.value = PriceHistoryScreenState.Failed(it)
            }
        }
    }

    // Alert
    private val _priceAlertStateFlow: MutableStateFlow<PriceAlertState> =
        MutableStateFlow(PriceAlertState.Idle)
    val priceAlertStateFlow
        get() = _priceAlertStateFlow.asStateFlow()

    fun createAlert(productId: String, storeId: Int, priceThreshold: Int) {
        // if (_priceAlertStateFlow.value !is PriceAlertState.Idle) return
        _priceAlertStateFlow.value = PriceAlertState.Loading

        viewModelScope.launch {
            runCatchingAPIFailure {
                alertService.createAlert(productId, storeId, priceThreshold)
            }.onSuccess {
                // this is just to increase the counter in profile screen
                val alert = PriceAlert(
                    id = it.value,
                    product = ProductAlert(
                        id = productId,
                        name = "",
                        imageUrl = ""
                    ),
                    store = StoreItem(
                        id = storeId,
                        name = ""
                    ),
                    priceThreshold = priceThreshold,
                    createdAt = LocalDateTime.now()
                )
                _priceAlertStateFlow.value = PriceAlertState.Created(alert)
                authRepository.addAlert(alert)
            }.onFailure {
                _priceAlertStateFlow.value = PriceAlertState.Error(it)
            }
        }
    }

    fun deleteAlert(alertId: String) {
        if (_priceAlertStateFlow.value is PriceAlertState.Loading) return
        _priceAlertStateFlow.value = PriceAlertState.Loading

        viewModelScope.launch {
            runCatchingAPIFailure { alertService.deleteAlert(alertId) }.onSuccess {
                _priceAlertStateFlow.value = PriceAlertState.Deleted
            }.onSuccess {
                authRepository.removeAlert(alertId)
            }.onFailure {
                _priceAlertStateFlow.value = PriceAlertState.Error(it)
            }
        }
    }
}