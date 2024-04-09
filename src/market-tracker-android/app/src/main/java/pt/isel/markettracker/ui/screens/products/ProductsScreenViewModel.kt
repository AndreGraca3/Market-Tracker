package pt.isel.markettracker.ui.screens.products

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Idle
import pt.isel.markettracker.domain.Loading
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.domain.product.StorePriceData

class ProductsScreenViewModel : ViewModel() {
    companion object {
        fun factory() = viewModelFactory {
            initializer { ProductsScreenViewModel() }
        }

        const val MAX_GRID_COLUMNS = 2
    }

    private val productsFlow: MutableStateFlow<IOState<List<ProductInfo>>> =
        MutableStateFlow(idle())

    val products: Flow<IOState<List<ProductInfo>>>
        get() = productsFlow.asStateFlow()

    fun fetchProducts(forceRefresh: Boolean = false) {
        if (productsFlow.value !is Idle && !forceRefresh) return

        productsFlow.value = loading()
        viewModelScope.launch {
            delay(2000L)
            productsFlow.value = loaded(
                Result.success(
                    (1..100).map {
                        ProductInfo(
                            it,
                            "Product $it",
                            "https://media.kabaz.pt/images/products/1/2/6/9/4/126946-1706041053.png",
                            StorePriceData(1, "Continente", Math.random() * 100),
                        )
                    }.toList().shuffled()
                )
            )
        }
    }
}