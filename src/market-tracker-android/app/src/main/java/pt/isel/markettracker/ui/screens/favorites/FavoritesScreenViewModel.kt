package pt.isel.markettracker.ui.screens.favorites

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
class FavoritesScreenViewModel @Inject constructor(
    private val productService: IProductService,
) : ViewModel() {
    private val _favoritesFlow: MutableStateFlow<FavoriteScreenState> =
        MutableStateFlow(FavoriteScreenState.Idle)
    val favorites
        get() = _favoritesFlow.asStateFlow()

    fun fetchFavorites() {
        if (_favoritesFlow.value !is FavoriteScreenState.Idle) return

        _favoritesFlow.value = FavoriteScreenState.Loading
        viewModelScope.launch {
            runCatchingAPIFailure {
                productService.getFavoriteProducts()
            }.onSuccess {
                _favoritesFlow.value = FavoriteScreenState.Loaded(it)
            }.onFailure {
                _favoritesFlow.value = FavoriteScreenState.Failed(it)
            }
        }
    }

    fun removeFromFavorites(productId: String) {
        val oldState = _favoritesFlow.value
        if (oldState !is FavoriteScreenState.Loaded) return

        val oldFavorites = oldState.favorites.toMutableList()

        viewModelScope.launch {
            runCatchingAPIFailure {
                productService.updateFavouriteProduct(productId, false)
            }.onSuccess {
                oldFavorites.removeIf { it.productId == productId }
                _favoritesFlow.value = FavoriteScreenState.Loaded(oldFavorites.toList())
            }.onFailure {
                _favoritesFlow.value = FavoriteScreenState.Failed(it)
            }
        }
    }
}