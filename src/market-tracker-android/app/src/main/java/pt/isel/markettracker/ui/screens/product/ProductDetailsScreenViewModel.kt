package pt.isel.markettracker.ui.screens.product

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.http.service.operations.alert.IAlertService
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.repository.auth.isLoggedIn
import pt.isel.markettracker.ui.screens.product.alert.PriceAlertState
import pt.isel.markettracker.ui.screens.product.rating.ProductPreferencesState
import java.time.LocalDateTime
import javax.inject.Inject

@HiltViewModel
class ProductDetailsScreenViewModel @Inject constructor(
    private val productService: IProductService,
    private val alertService: IAlertService,
    private val authRepository: IAuthRepository
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
    }

    private fun fetchProductPrices(productId: String) {
        val screenState = _stateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadingProductDetails ||
            screenState.prices != null
        ) return

        loadDetailAndSetScreenStateIfReady(apiCall = { productService.getProductPrices(productId) }) {
            copy(prices = it)
        }
    }

    private fun fetchProductStats(productId: String) {
        val screenState = _stateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadingProductDetails ||
            screenState.stats != null
        ) return

        loadDetailAndSetScreenStateIfReady(apiCall = { productService.getProductStats(productId) }) {
            copy(stats = it)
        }
    }

    private var currentReviewsPage by mutableIntStateOf(1)

    fun fetchProductReviews(productId: String) {
        val screenState = _stateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadedDetails ||
            (screenState.paginatedReviews != null && !screenState.paginatedReviews.hasMore)
        ) return

        _stateFlow.value = screenState.toLoadingReviews()

        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                productService.getProductReviews(
                    productId,
                    currentReviewsPage++
                )
            }

            res.onSuccess {
                val paginatedReviews =
                    (screenState.paginatedReviews?.items?.plus(it.items)) ?: it.items
                _stateFlow.value = ProductDetailsScreenState.LoadedDetails(
                    product = screenState.product,
                    prices = screenState.prices,
                    stats = screenState.stats,
                    paginatedReviews = it.copy(items = paginatedReviews)
                )
            }
        }
    }

    private val _prefsStateFlow: MutableStateFlow<ProductPreferencesState> =
        MutableStateFlow(ProductPreferencesState.Idle)
    val prefsStateFlow
        get() = _prefsStateFlow.asStateFlow()

    private fun fetchProductPreferences(productId: String) {
        val prefsState = _prefsStateFlow.value
        if (prefsState !is ProductPreferencesState.Idle) return
        if (!authRepository.authState.value.isLoggedIn()) {
            _prefsStateFlow.value = ProductPreferencesState.Unauthenticated
            return
        }

        _prefsStateFlow.value = ProductPreferencesState.Loading

        viewModelScope.launch {
            val res = runCatchingAPIFailure { productService.getProductPreferences(productId) }

            res.onSuccess {
                _prefsStateFlow.value = ProductPreferencesState.Loaded(it)
            }.onFailure {
                _prefsStateFlow.value = ProductPreferencesState.Failed(it)
            }
        }
    }

    fun submitUserRating(productId: String, rating: Int, comment: String) {
        val screenState = _stateFlow.value
        val prefsState = _prefsStateFlow.value
        if (screenState !is ProductDetailsScreenState.LoadedDetails ||
            prefsState !is ProductPreferencesState.Loaded
        ) return

        _prefsStateFlow.value = ProductPreferencesState.Loading

        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                productService.submitProductReview(
                    productId,
                    rating,
                    comment.takeIf { it.isNotBlank() }
                )
            }

            res.onSuccess { newReview ->
                val newCounts = screenState.stats.counts.ratings + 1
                val newRating = (screenState.stats.averageRating *
                        screenState.stats.counts.ratings + rating) / newCounts

                var newState = screenState.copy(
                    stats = screenState.stats.copy(
                        averageRating = newRating,
                        counts = screenState.stats.counts.copy(ratings = newCounts)
                    )
                )

                if (screenState.paginatedReviews != null) {
                    var isNewReview = true

                    val reviews = screenState.paginatedReviews.items.map {
                        if (it.id == newReview.id) {
                            isNewReview = false
                            newReview
                        } else it
                    }

                    newState = newState.copy(
                        paginatedReviews = screenState.paginatedReviews.copy(
                            items = if (isNewReview) listOf(newReview) +
                                    screenState.paginatedReviews.items
                            else reviews
                        )
                    )
                }

                _stateFlow.value = newState

                _prefsStateFlow.value = prefsState.copy(
                    prefsState.preferences.copy(review = newReview)
                )
            }.onFailure {
                _prefsStateFlow.value = ProductPreferencesState.Failed(it)
            }
        }
    }

    fun deleteReview(productId: String) {
        val initialScreenState = _stateFlow.value
        val initialPrefsState = _prefsStateFlow.value
        if (initialScreenState !is ProductDetailsScreenState.LoadedDetails ||
            initialPrefsState !is ProductPreferencesState.Loaded ||
            initialPrefsState.preferences.review == null
        ) return

        val newCounts = initialScreenState.stats.counts.ratings - 1
        val newRating = if (newCounts == 0) 0.0 else {
            (initialScreenState.stats.averageRating * initialScreenState.stats.counts.ratings -
                    initialPrefsState.preferences.review.rating) / newCounts
        }

        // Optimistic update
        _stateFlow.value = initialScreenState.copy(
            paginatedReviews = initialScreenState.paginatedReviews?.copy(
                items = initialScreenState.paginatedReviews.items.filterNot {
                    it.id == initialPrefsState.preferences.review.id
                }
            ),
            stats = initialScreenState.stats.copy(
                averageRating = newRating,
                counts = initialScreenState.stats.counts.copy(
                    ratings = newCounts
                )
            )
        )

        _prefsStateFlow.value = initialPrefsState.copy(
            initialPrefsState.preferences.copy(review = null)
        )

        viewModelScope.launch {
            runCatchingAPIFailure {
                productService.deleteProductReview(productId)
            }.onFailure {
                _stateFlow.value = initialScreenState
                _prefsStateFlow.value = initialPrefsState
            }
        }
    }

    fun submitFavourite(isFavourite: Boolean) {
        val currScreenState = _stateFlow.value
        val currPrefsState = _prefsStateFlow.value
        if (currScreenState !is ProductDetailsScreenState.LoadedDetails ||
            currPrefsState !is ProductPreferencesState.Loaded
        ) return

        // Optimistic update
        _prefsStateFlow.value = currPrefsState.copy(
            currPrefsState.preferences.copy(isFavourite = isFavourite)
        )

        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                productService.updateFavouriteProduct(currScreenState.product.id, isFavourite)
            }

            res.onFailure {
                _prefsStateFlow.value = currPrefsState
            }
        }
    }

    // Alert
    private val _priceAlertStateFlow: MutableStateFlow<PriceAlertState> =
        MutableStateFlow(PriceAlertState.Idle)
    val priceAlertStateFlow
        get() = _priceAlertStateFlow.asStateFlow()

    fun createAlert(productId: String, storeId: Int, priceThreshold: Int) {
        if (_priceAlertStateFlow.value is PriceAlertState.Loading) return
        _priceAlertStateFlow.value = PriceAlertState.Loading

        viewModelScope.launch {
            runCatchingAPIFailure {
                alertService.createAlert(productId, storeId, priceThreshold)
            }.onSuccess {
                val alert = PriceAlert(
                    id = it.value,
                    productId = productId,
                    storeId = storeId,
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