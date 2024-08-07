package pt.isel.markettracker.ui.screens.listDetails

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material3.DrawerValue
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.rememberDrawerState
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import kotlinx.coroutines.launch
import pt.isel.markettracker.ui.components.modalDrawer.MarketTrackerRightModalDrawer
import pt.isel.markettracker.ui.screens.listDetails.buttons.OpenUserMenuButton
import pt.isel.markettracker.ui.screens.listDetails.components.MembersModal
import pt.isel.markettracker.ui.screens.listDetails.list.ProductListView
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun ListDetailsScreenView(
    state: ListDetailsScreenState,
    onAddUsersToListRequested: () -> Unit,
    onRemoveUserFromLisTRequested: (String) -> Unit,
    fetchListDetails: () -> Unit,
    isRefreshing: Boolean,
    changeProductCount: (String, Int, Int) -> Unit,
    deleteProductFromList: (String) -> Unit,
) {
    val drawerState = rememberDrawerState(initialValue = DrawerValue.Closed)
    val scope = rememberCoroutineScope()

    Scaffold(
        topBar =
        {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .background(Color.Red)
                    .padding(10.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                Box(
                    modifier = Modifier.fillMaxWidth()
                ) {
                    HeaderLogo(
                        modifier = Modifier
                            .align(alignment = Alignment.CenterStart)
                            .size(48.dp)
                    )
                    Text(
                        text = "Carrinho 🛒",
                        color = Color.White,
                        fontFamily = mainFont,
                        fontSize = 30.sp,
                        modifier = Modifier
                            .align(alignment = Alignment.Center)
                    )
                    OpenUserMenuButton(
                        onClick = {
                            scope.launch {
                                if (drawerState.isOpen) {
                                    drawerState.close()
                                } else drawerState.open()
                            }
                        },
                        modifier = Modifier.align(alignment = Alignment.CenterEnd)
                    )
                }
            }
        }
    ) { paddingValues ->
        Box(
            modifier = Modifier.fillMaxWidth(),
            contentAlignment = Alignment.TopCenter
        ) {
            MarketTrackerRightModalDrawer(
                drawerState = drawerState,
                content = {
                    Box(
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(paddingValues),
                        contentAlignment = Alignment.TopCenter
                    ) {
                        ProductListView(
                            state = state,
                            isRefreshing = isRefreshing,
                            isInCheckBoxMode = false,
                            fetchListDetails = fetchListDetails,
                            onDeleteProductFromListRequested = { entryId ->
                                deleteProductFromList(entryId)
                            },
                            onQuantityChangeRequest = { entryId, storeId, newQuantity ->
                                changeProductCount(
                                    entryId,
                                    storeId,
                                    newQuantity
                                )
                            }
                        )
                    }
                },
                drawerContent = {
                    Box(
                        modifier = Modifier
                            .fillMaxSize()
                            .padding(paddingValues),
                        contentAlignment = Alignment.CenterEnd
                    ) {
                        MembersModal(
                            state = state,
                            onAddUsersToListRequested = onAddUsersToListRequested,
                            onRemoveUserFromLisTRequested = onRemoveUserFromLisTRequested
                        )
                    }
                }
            )
        }
    }
}