package pt.isel.markettracker.ui.screens.listDetails

import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries

sealed class ListDetailsScreenState {
    data object Idle : ListDetailsScreenState()

    data object Loading : ListDetailsScreenState()

    data class Success(
        val shoppingListEntries: ShoppingListEntries,
    ) : ListDetailsScreenState()

    data class Failed(val error: Throwable) : ListDetailsScreenState()
}

fun ListDetailsScreenState.extractDetails() =
    when (this) {
        is ListDetailsScreenState.Success -> shoppingListEntries
        else -> null
    }