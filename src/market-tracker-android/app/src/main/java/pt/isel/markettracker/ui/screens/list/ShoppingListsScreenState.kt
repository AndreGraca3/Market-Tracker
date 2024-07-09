package pt.isel.markettracker.ui.screens.list

import pt.isel.markettracker.domain.model.list.ShoppingList

sealed class ShoppingListsScreenState {
    data object Idle : ShoppingListsScreenState()

    data object Loading : ShoppingListsScreenState()

    data class Loaded(
        val shoppingLists: List<ShoppingList>,
    ) : ShoppingListsScreenState()

    data class Editing(
        val shoppingLists: List<ShoppingList>,
        val currentListEditing: ShoppingList,
    ) : ShoppingListsScreenState()

    data class WaitFinishCreation(
        val shoppingLists: List<ShoppingList>,
    ) : ShoppingListsScreenState()

    data class Failed(val error: Throwable) : ShoppingListsScreenState()
}

fun ShoppingListsScreenState.extractShoppingLists() =
    when (this) {
        is ShoppingListsScreenState.Loaded -> shoppingLists
        is ShoppingListsScreenState.Editing -> shoppingLists
        is ShoppingListsScreenState.WaitFinishCreation -> shoppingLists
        else -> emptyList()
    }