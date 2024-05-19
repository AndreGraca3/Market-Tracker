package pt.isel.markettracker.ui.screens.product

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Idle
import pt.isel.markettracker.domain.fail
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.domain.model.market.inventory.product.ProductStats
import pt.isel.markettracker.domain.model.market.price.CompanyPrices
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.ui.screens.products.ProductsScreenState
import javax.inject.Inject

@HiltViewModel
class ProductDetailsScreenViewModel @Inject constructor(
    private val productService: IProductService
) : ViewModel() {

    private val _stateFlow: MutableStateFlow<ProductDetailsScreenState> =
        MutableStateFlow(ProductDetailsScreenState.Idle)
    val stateFlow
        get() = _stateFlow.asStateFlow()

    fun fetchProductById(id: String) {
        if (_stateFlow.value !is ProductDetailsScreenState.Idle) return
        _stateFlow.value = ProductDetailsScreenState.LoadingProduct

        viewModelScope.launch {
            val res = kotlin.runCatching { productService.getProductById(id) }

            res.onSuccess {
                _stateFlow.value = ProductDetailsScreenState.LoadingProductDetails(it)
            }

            res.onFailure {
                _stateFlow.value = ProductDetailsScreenState.Failed(it)
            }
        }
    }

    private val pricesFlow = MutableStateFlow<IOState<List<CompanyPrices>>>(idle())
    val prices
        get() = pricesFlow

    fun fetchProductPrices(id: String) {
        if (pricesFlow.value !is Idle) return
        pricesFlow.value = loading()

        viewModelScope.launch {
            val res = kotlin.runCatching { productService.getProductPrices(id) }
            pricesFlow.value = when (res.isSuccess) {
                true -> loaded(res)
                false -> fail(res.exceptionOrNull()!!)
            }
        }
    }

    private val statsFlow = MutableStateFlow<IOState<ProductStats>>(idle())
    val stats
        get() = statsFlow

    fun fetchProductStats(id: String) {
        if (statsFlow.value !is Idle) return
        statsFlow.value = loading()

        viewModelScope.launch {
            val res = kotlin.runCatching { productService.getProductStats(id) }
            statsFlow.value = when (res.isSuccess) {
                true -> loaded(res)
                false -> fail(res.exceptionOrNull()!!)
            }
        }
    }

    private val preferencesFlow = MutableStateFlow<IOState<ProductPreferences>>(idle())
    val preferences
        get() = preferencesFlow

    fun fetchProductPreferences(id: String) {
        if (preferencesFlow.value !is Idle) return
        preferencesFlow.value = loading()

        viewModelScope.launch {
            val res = kotlin.runCatching { productService.getProductPreferences(id) }
            preferencesFlow.value = when (res.isSuccess) {
                true -> loaded(res)
                false -> fail(res.exceptionOrNull()!!)
            }
        }
    }

    private val alertsFlow = MutableStateFlow<IOState<List<PriceAlert>>>(idle())
    val alerts
        get() = alertsFlow

    fun fetchProductAlerts(id: String) {
        if (alertsFlow.value !is Idle) return
        alertsFlow.value = loading()

        viewModelScope.launch {
            val res = kotlin.runCatching { productService.getProductAlerts(id) }
            alertsFlow.value = when (res.isSuccess) {
                true -> loaded(res)
                false -> fail(res.exceptionOrNull()!!)
            }
        }
    }

    private val reviewsFlow = MutableStateFlow<IOState<List<ProductReview>>>(idle())
    val reviews
        get() = reviewsFlow

    fun fetchProductReviews(id: String) {
        // TODO: Implement this function
    }
}