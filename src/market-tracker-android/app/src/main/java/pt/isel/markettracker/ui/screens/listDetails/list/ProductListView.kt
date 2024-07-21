package pt.isel.markettracker.ui.screens.listDetails.list

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.listDetails.ListDetailsScreenState
import pt.isel.markettracker.ui.screens.listDetails.cards.ProductListCard
import pt.isel.markettracker.ui.screens.listDetails.components.DisplayListCount
import pt.isel.markettracker.ui.screens.listDetails.extractShoppingListEntries
import pt.isel.markettracker.ui.screens.listDetails.extractShoppingListSocial

@Composable
fun ProductListView(
    state: ListDetailsScreenState,
    isRefreshing: Boolean,
    isInCheckBoxMode: Boolean,
    fetchListDetails: () -> Unit,
    onDeleteProductFromListRequested: (String) -> Unit,
    onQuantityChangeRequest: (String, Int, Int) -> Unit,
) {
    when (state) {
        is ListDetailsScreenState.PartiallyLoaded,
        is ListDetailsScreenState.Loaded,
        is ListDetailsScreenState.Editing,
        is ListDetailsScreenState.WaitingForEditing,
        -> {
            val listItems = state.extractShoppingListEntries()

            Column {
                DisplayListCount(
                    totalPrice = listItems.totalPrice,
                    amountOfProducts = listItems.totalProducts
                )

                if (listItems.entries.isEmpty()) {
                    Box(
                        modifier = Modifier.fillMaxSize(),
                        contentAlignment = Alignment.Center
                    ) {
                        Text(
                            text = "Esta lista está vazia."
                        )
                    }
                }

                PullToRefreshLazyColumn(
                    isRefreshing = isRefreshing,
                    onRefresh = fetchListDetails,
                ) {
                    Column(
                        modifier = Modifier
                            .fillMaxSize(),
                        horizontalAlignment = Alignment.CenterHorizontally
                    ) {
                        LazyColumn(
                            verticalArrangement = Arrangement.spacedBy(10.dp),
                            contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
                        ) {
                            items(listItems.entries.size) { index ->
                                val item = listItems.entries[index]
                                ProductListCard(
                                    productEntry = item,
                                    isEditable = state.extractShoppingListSocial()?.archivedAt == null,
                                    isEditing = state is ListDetailsScreenState.Editing,
                                    onQuantityIncreaseRequest = {
                                        onQuantityChangeRequest(
                                            item.id,
                                            item.productOffer.storeOffer.store.id,
                                            item.quantity + 1
                                        )
                                    },
                                    onQuantityDecreaseRequest = {
                                        val newQuantity = item.quantity - 1
                                        if (newQuantity <= 0) {
                                            onDeleteProductFromListRequested(item.id)
                                        } else {
                                            onQuantityChangeRequest(
                                                item.id,
                                                item.productOffer.storeOffer.store.id,
                                                item.quantity - 1
                                            )
                                        }
                                    },
                                    isLoading = state is ListDetailsScreenState.WaitingForEditing && state.entryId == item.id,
                                    loadingContent = {
                                        LoadingIcon()
                                    }
                                )
                            }
                        }
                    }
                }
            }
        }

        is ListDetailsScreenState.Loading -> {
            Box(
                contentAlignment = Alignment.Center,
                modifier = Modifier.fillMaxSize()
            ) {
                LoadingIcon(text = "Carregando a lista...")
            }
        }

        else -> {}
    }
}