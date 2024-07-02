package pt.isel.markettracker.ui.screens.productsList

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
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries
import pt.isel.markettracker.http.service.operations.list.listEntry.IListEntryService
import javax.inject.Inject

@HiltViewModel
class ListProductDetailsScreenViewModel @Inject constructor(
    private val listEntryService: IListEntryService
) : ViewModel() {

    private val listProductFlow: MutableStateFlow<IOState<ShoppingListEntries>> =
        MutableStateFlow(idle())
    val listProduct
        get() = listProductFlow.asStateFlow()

    fun fetchProducts(forceRefresh: Boolean = false) {
        if (listProductFlow.value !is Idle && !forceRefresh) return

        listProductFlow.value = loading()
        viewModelScope.launch {
            val res = kotlin.runCatching {
                listEntryService.getListEntries("")
            }
            listProductFlow.value = when (res.isSuccess) {
                true -> loaded(res)
                false -> fail(res.exceptionOrNull()!!) // TODO: handle error
            }
        }
    }
}