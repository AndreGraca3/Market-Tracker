package pt.isel.markettracker.ui.screens.products

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.service.operations.product.IProductService
import pt.isel.markettracker.ui.screens.ProductsQuery
import pt.isel.markettracker.ui.screens.ProductsScreenState
import pt.isel.markettracker.ui.screens.extractProducts
import javax.inject.Inject

@HiltViewModel
class ProductsScreenViewModel @Inject constructor(
    private val productService: IProductService
) : ViewModel() {
    companion object {
        const val MAX_GRID_COLUMNS = 2
    }

    // actual listed products
    private val _stateFlow: MutableStateFlow<ProductsScreenState> =
        MutableStateFlow(ProductsScreenState.Idle)
    val stateFlow
        get() = _stateFlow.asStateFlow()

    private var currentPage by mutableIntStateOf(1)
    private var hasMore by mutableStateOf(true)

    fun fetchProducts(
        productsQuery: ProductsQuery,
        forceRefresh: Boolean = false
    ) {
        if (_stateFlow.value !is ProductsScreenState.Idle) return

        val oldProducts = _stateFlow.value.extractProducts()
        _stateFlow.value = ProductsScreenState.Loading
        currentPage = 1
        hasMore = true

        viewModelScope.launch {
            val res = kotlin.runCatching {
                productService.getProducts(
                    currentPage,
                    searchQuery = productsQuery.searchTerm,
                    sortOption = productsQuery.sortOption.name
                )
            }

            _stateFlow.value = when (res.isSuccess) {
                true -> {
                    val resValue = res.getOrThrow()
                    if (!resValue.hasMore) hasMore = false else currentPage++
                    val allProducts = oldProducts + resValue.items
                    ProductsScreenState.Loaded(productsQuery, allProducts)
                }

                false -> ProductsScreenState.Error(productsQuery, res.exceptionOrNull()!!)
            }
        }
    }

    fun loadMoreProducts(productsQuery: ProductsQuery) {
        if (!hasMore || _stateFlow.value !is ProductsScreenState.Loaded) return

        val oldProducts = _stateFlow.value.extractProducts()
        _stateFlow.value = ProductsScreenState.Loading

        viewModelScope.launch {
            val res = kotlin.runCatching {
                productService.getProducts(
                    currentPage,
                    searchQuery = productsQuery.searchTerm,
                    sortOption = productsQuery.sortOption.name
                )
            }

            _stateFlow.value = when (res.isSuccess) {
                true -> {
                    val resValue = res.getOrThrow()
                    if (!resValue.hasMore) hasMore = false else currentPage++
                    val allProducts = oldProducts + resValue.items
                    ProductsScreenState.Loaded(productsQuery, allProducts)
                }

                false -> ProductsScreenState.Error(productsQuery, res.exceptionOrNull()!!)
            }
        }
    }
}