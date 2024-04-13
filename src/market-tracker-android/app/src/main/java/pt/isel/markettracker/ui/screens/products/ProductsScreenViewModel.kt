package pt.isel.markettracker.ui.screens.products

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
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.http.service.operations.product.IProductService
import javax.inject.Inject

@HiltViewModel
class ProductsScreenViewModel @Inject constructor(
    private val productService: IProductService
) : ViewModel() {
    companion object {
        const val MAX_GRID_COLUMNS = 2
    }

    // search and filters
    private val searchQueryFlow: MutableStateFlow<String> = MutableStateFlow("")
    val searchQuery
        get() = searchQueryFlow.asStateFlow()

    private val filtersFlow: MutableStateFlow<ProductsFilters> =
        MutableStateFlow(ProductsFilters())
    val filters
        get() = filtersFlow.asStateFlow()

    fun onQueryChange(query: String) {
        searchQueryFlow.value = query
    }

    // actual listed products
    private val productsFlow: MutableStateFlow<IOState<List<ProductInfo>>> =
        MutableStateFlow(idle())
    val products
        get() = productsFlow.asStateFlow()

    fun fetchProducts(forceRefresh: Boolean = false) {
        if (productsFlow.value !is Idle && !forceRefresh) return

        productsFlow.value = loading()
        viewModelScope.launch {
            val res = kotlin.runCatching {
                productService.getProducts(
                    if (searchQueryFlow.value.isBlank()) null else searchQuery.value
                )
            }
            productsFlow.value = when (res.isSuccess) {
                true -> loaded(res)
                false -> fail(res.exceptionOrNull()!!) // TODO: handle error
            }
        }
    }
}

data class ProductsFilters(
    val brandId: Int? = null,
    val categoryId: Int? = null,
    val minRating: String? = null,
    val maxRating: String? = null
)
