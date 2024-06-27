package pt.isel.markettracker.ui.screens.products.list

import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer

sealed class AddToListState {
    data object Idle : AddToListState()

    data class SelectingList(val productOffer: ProductOffer) : AddToListState()

    data class AddingToList(
        val offer: ProductOffer,
        val listId: String,
    ) : AddToListState()

    sealed class Done : AddToListState()

    data class Failed(val error: Throwable) : Done()

    data class Success(
        val offer: ProductOffer,
        val listId: String,
    ) : Done()
}

fun AddToListState.extractOffer() =
    when (this) {
        is AddToListState.SelectingList -> productOffer
        is AddToListState.AddingToList -> offer
        is AddToListState.Success -> offer
        else -> null
    }

fun AddToListState.extractListId() =
    when (this) {
        is AddToListState.AddingToList -> listId
        is AddToListState.Success -> listId
        else -> null
    }

fun AddToListState.extractError() =
    when (this) {
        is AddToListState.Failed -> error
        else -> null
    }

fun AddToListState.AddingToList.toSuccess() = AddToListState.Success(offer, listId)