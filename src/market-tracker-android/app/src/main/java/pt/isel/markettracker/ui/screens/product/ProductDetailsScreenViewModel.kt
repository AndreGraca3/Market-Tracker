package pt.isel.markettracker.ui.screens.product

import android.util.Log
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.setValue
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
class ProductDetailsScreenViewModel @Inject constructor(
    private val productService: IProductService
) : ViewModel() {

    private val _stateFlow: MutableStateFlow<ProductDetailsScreenState> =
        MutableStateFlow(ProductDetailsScreenState.Idle)
    val stateFlow
        get() = _stateFlow.asStateFlow()

    fun fetchProductById(productId: String) {
        if (_stateFlow.value !is ProductDetailsScreenState.Idle) return
        _stateFlow.value = ProductDetailsScreenState.LoadingProduct

        viewModelScope.launch {
            val res = runCatchingAPIFailure { productService.getProductById(productId) }

            res.onSuccess {
                _stateFlow.value = ProductDetailsScreenState.LoadedProduct(it)
            }

            res.onFailure {
                _stateFlow.value = ProductDetailsScreenState.FailedToLoadProduct(it)
            }
        }
    }

    fun fetchProductDetails(productId: String) {
        val screenState = _stateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadedProduct) return
        _stateFlow.value = ProductDetailsScreenState.LoadingProductDetails(screenState.product)

        fetchProductStats(productId)
        fetchProductPreferences(productId)
        fetchProductPrices(productId)
        fetchProductAlerts(productId)
    }

    fun fetchProductPrices(productId: String) {
        val screenState = _stateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadingProductDetails ||
            screenState.prices != null
        ) return

        loadDetailAndSetScreenStateIfReady(apiCall = { productService.getProductPrices(productId) }) {
            copy(prices = it)
        }
    }

    fun fetchProductStats(productId: String) {
        val screenState = _stateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadingProductDetails ||
            screenState.stats != null
        ) return

        loadDetailAndSetScreenStateIfReady(apiCall = { productService.getProductStats(productId) }) {
            copy(stats = it)
        }
    }

    fun fetchProductPreferences(productId: String) {
        val screenState = _stateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadingProductDetails ||
            screenState.preferences != null
        ) return

        loadDetailAndSetScreenStateIfReady(apiCall = {
            productService.getProductPreferences(productId)
        }) {
            copy(preferences = it)
        }
    }

    fun fetchProductAlerts(productId: String) {
        val screenState = _stateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadingProductDetails ||
            screenState.alerts != null
        ) return

        loadDetailAndSetScreenStateIfReady(apiCall = { productService.getProductAlerts(productId) }) {
            copy(alerts = it)
        }
    }

    private var currentReviewsPage by mutableIntStateOf(1)

    fun fetchProductReviews(productId: String) {
        val screenState = _stateFlow.value
        if (screenState is ProductDetailsScreenState.LoadingReviews ||
            screenState !is ProductDetailsScreenState.LoadedDetails ||
            (screenState.paginatedReviews != null && !screenState.paginatedReviews.hasMore)
        ) return

        Log.v("Reviews", "fetching product reviews")

        _stateFlow.value = screenState.toLoadingReviews()

        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                productService.getProductReviews(
                    productId,
                    currentReviewsPage++
                )
            }

            res.onSuccess {
                val paginatedReviews = (screenState.paginatedReviews?.items?.plus(it.items)) ?: it.items
                _stateFlow.value = ProductDetailsScreenState.LoadedDetails(
                    product = screenState.product,
                    prices = screenState.prices,
                    stats = screenState.stats,
                    preferences = screenState.preferences,
                    alerts = screenState.alerts,
                    paginatedReviews = it.copy(items = paginatedReviews)
                )
            }
        }
    }

    fun submitUserRating(productId: String, rating: Int, text: String) {
        Log.v("Reviews", "submitting user rating")
        val screenState = _stateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadedDetails) return
        _stateFlow.value = screenState.toSubmittingReview()

        Log.v("Reviews", "actually submitting user rating")

        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                productService.submitProductReview(
                    productId,
                    rating,
                    text
                )
            }

            res.onSuccess {
                Log.v("Reviews", "submitted user rating")
                _stateFlow.value =
                    screenState.copy(
                        paginatedReviews = screenState.paginatedReviews?.copy(
                            items = screenState.paginatedReviews.items + it
                        )
                    )
            }.onFailure {
                _stateFlow.value = ProductDetailsScreenState.Failed(it)
            }
        }
    }

    private inline fun <T> loadDetailAndSetScreenStateIfReady(
        crossinline apiCall: suspend () -> T,
        crossinline updateState: ProductDetailsScreenState.LoadingProductDetails.(T) -> ProductDetailsScreenState.LoadingProductDetails
    ) {
        viewModelScope.launch {
            val res = runCatchingAPIFailure { apiCall() }

            res.onSuccess {
                val currState = _stateFlow.value
                if (currState !is ProductDetailsScreenState.LoadingProductDetails) return@onSuccess

                _stateFlow.value = currState.updateState(it).toLoadedDetailsIfReady()
            }.onFailure {
                _stateFlow.value = ProductDetailsScreenState.Failed(it)
            }
        }
    }
}