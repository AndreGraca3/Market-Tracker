package pt.isel.markettracker.ui.screens.list.shoppingLists

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.list.shoppingLists.grid.ListGrid
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun ListScreenView(
    state: ShoppingListsScreenState,
    fetchLists: (Boolean) -> Unit,
    onArchiveList: (Int) -> Unit,
    onDeleteList: (Int) -> Unit,
    onListClick: (Int) -> Unit
) {
    var isRefreshing by remember { mutableStateOf(false) }

    LaunchedEffect(Unit) {
        fetchLists(false)
    }

    LaunchedEffect(state) {
        if (state !is ShoppingListsScreenState.Loading) {
            isRefreshing = false
        }
    }

    Scaffold(
        topBar = {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .background(Color.Red)
                    .padding(10.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                HeaderLogo()
                Text(
                    "As minhas listas 📝",
                    color = Color.White,
                    fontFamily = mainFont,
                    fontSize = 30.sp
                )
            }
        }
    ) { paddingValues ->
        PullToRefreshLazyColumn(
            isRefreshing = isRefreshing,
            onRefresh = {
                isRefreshing = true
                fetchLists(true)
            },
            modifier = Modifier
                .padding(paddingValues)
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .background(Color.LightGray),
                horizontalAlignment = Alignment.CenterHorizontally,
            ) {
                ListGrid(
                    state = state,
                    onArchiveListRequest = onArchiveList,
                    onDeleteListRequest = onDeleteList,
                    onListDetailsRequest = onListClick
                )
            }
        }
    }
}