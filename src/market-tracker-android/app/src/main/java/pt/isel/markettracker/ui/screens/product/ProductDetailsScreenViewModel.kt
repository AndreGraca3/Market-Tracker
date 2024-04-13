package pt.isel.markettracker.ui.screens.product

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.fail
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.http.service.operations.product.IProductService
import javax.inject.Inject

@HiltViewModel
class ProductDetailsScreenViewModel @Inject constructor(
    private val productService: IProductService
) : ViewModel() {

    private val productFlow = MutableStateFlow<IOState<ProductInfo>>(idle())
    val product
        get() = productFlow

    fun getProductById(id: String) {
        productFlow.value = loading()

        viewModelScope.launch {
            val res = kotlin.runCatching { productService.getProductById(id) }
            productFlow.value = when (res.isSuccess) {
                true -> loaded(res)
                false -> fail(res.exceptionOrNull()!!)
            }
        }
    }

    fun getProductPrices() {
        // TODO: Implement this function
    }
}