package pt.isel.markettracker.ui.screens.listDetails

import pt.isel.markettracker.domain.model.account.PaginatedClientItem
import pt.isel.markettracker.domain.model.list.ShoppingListSocial
import pt.isel.markettracker.domain.model.list.listEntry.ShoppingListEntries

sealed class ListDetailsScreenState {
    data object Idle : ListDetailsScreenState()

    data object Loading : ListDetailsScreenState()

    data class PartiallyLoaded(
        val shoppingListEntries: ShoppingListEntries,
    ) : ListDetailsScreenState()

    data class Loaded(
        val shoppingListEntries: ShoppingListEntries,
        val shoppingListSocial: ShoppingListSocial,
        val result: PaginatedClientItem?,
    ) : ListDetailsScreenState()

    data class SearchingUsers(
        val shoppingListEntries: ShoppingListEntries,
        val shoppingListSocial: ShoppingListSocial,
    ) : ListDetailsScreenState()

    data class Editing(
        val shoppingListEntries: ShoppingListEntries,
        val shoppingListSocial: ShoppingListSocial,
    ) : ListDetailsScreenState()

    data class WaitingForEditing(
        val shoppingListEntries: ShoppingListEntries,
        val shoppingListSocial: ShoppingListSocial,
        val listId: String,
    ) : ListDetailsScreenState()

    data class Failed(val error: Throwable) : ListDetailsScreenState()
}

fun ListDetailsScreenState.extractShoppingListEntries() =
    when (this) {
        is ListDetailsScreenState.Loaded -> shoppingListEntries
        is ListDetailsScreenState.Editing -> shoppingListEntries
        is ListDetailsScreenState.WaitingForEditing -> shoppingListEntries
        is ListDetailsScreenState.PartiallyLoaded -> shoppingListEntries
        else -> ShoppingListEntries(
            entries = emptyList(),
            totalPrice = 0,
            totalProducts = 0
        )
    }

fun ListDetailsScreenState.extractShoppingListSocial() =
    when (this) {
        is ListDetailsScreenState.Loaded -> shoppingListSocial
        is ListDetailsScreenState.SearchingUsers -> shoppingListSocial
        is ListDetailsScreenState.Editing -> shoppingListSocial
        is ListDetailsScreenState.WaitingForEditing -> shoppingListSocial
        else -> null
    }

fun ListDetailsScreenState.extractSearchResult() =
    when (this) {
        is ListDetailsScreenState.Loaded -> result
        else -> null
    }