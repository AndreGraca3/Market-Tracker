package pt.isel.markettracker.ui.screens.product

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateMapOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Idle
import pt.isel.markettracker.domain.fail
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.domain.price.StorePrice
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.domain.product.ProductPreferences
import pt.isel.markettracker.domain.product.ProductReview
import pt.isel.markettracker.domain.product.ProductStats
import pt.isel.markettracker.http.models.price.CompanyPrices
import pt.isel.markettracker.http.service.operations.product.IProductService
import javax.inject.Inject

@HiltViewModel
class ProductDetailsScreenViewModel @Inject constructor(
    private val productService: IProductService
) : ViewModel() {

    private val productFlow = MutableStateFlow<IOState<ProductInfo>>(idle())
    val product
        get() = productFlow

    fun fetchProductById(id: String) {
        if (productFlow.value !is Idle) return
        productFlow.value = loading()

        viewModelScope.launch {
            val res = kotlin.runCatching { productService.getProductById(id) }
            productFlow.value = when (res.isSuccess) {
                true -> loaded(res)
                false -> fail(res.exceptionOrNull()!!)
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

    private val reviewsFlow = MutableStateFlow<IOState<List<ProductReview>>>(idle())
    val reviews
        get() = reviewsFlow

    fun fetchProductReviews(id: String) {
        // TODO: Implement this function
    }
}