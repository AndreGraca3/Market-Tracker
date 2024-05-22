package pt.isel.markettracker.ui.screens.list.listProductItem

import androidx.lifecycle.ViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.http.service.operations.list.IListService
import javax.inject.Inject

class ListProductDetailsScreenViewModel @Inject constructor(
    private val listService: IListService
) : ViewModel() {

//    private val listproductFlow: MutableStateFlow<IOState<ListProduct>>(idle())
//    val listproduct
//        get() = listproductFlow


}