package pt.isel.markettracker.ui.screens.priceHistory

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import javax.inject.Inject

@HiltViewModel
class PriceHistoryScreenViewModel @Inject constructor(
    private val productService: IProductService,
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


}