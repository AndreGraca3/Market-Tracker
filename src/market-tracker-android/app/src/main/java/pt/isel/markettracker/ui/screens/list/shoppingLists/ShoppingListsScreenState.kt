package pt.isel.markettracker.ui.screens.list.shoppingLists

import pt.isel.markettracker.domain.model.list.ShoppingList

sealed class ShoppingListsScreenState {
    data object Idle : ShoppingListsScreenState()

    data object Loading : ShoppingListsScreenState()

    data class Loaded(
        val shoppingLists: List<ShoppingList>,
    ) : ShoppingListsScreenState()

    data class Failed(val error: Throwable) : ShoppingListsScreenState()
}

fun ShoppingListsScreenState.extractShoppingLists() =
    when (this) {
        is ShoppingListsScreenState.Loaded -> shoppingLists
        else -> emptyList()
    }