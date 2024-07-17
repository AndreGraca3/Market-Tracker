package pt.isel.markettracker.ui.screens.users

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.sp
import pt.isel.markettracker.ui.components.MarketTrackerHeader
import pt.isel.markettracker.ui.components.buttons.MarketTrackerBackButton
import pt.isel.markettracker.ui.components.common.PullToRefreshLazyColumn
import pt.isel.markettracker.ui.screens.users.states.UsersScreenState
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun UsersScreenView(
    state: UsersScreenState,
    query: String,
    onQueryChangeRequested: (String) -> Unit,
    onFetchUsersRequested: () -> Unit,
    onFetchMoreUsersRequested: () -> Unit,
    onAddUserToListRequested: (String) -> Unit,
    onBackRequested: () -> Unit,
) {
    var isRefreshing by rememberSaveable { mutableStateOf(false) }

    LaunchedEffect(state) {
        if (state !is UsersScreenState.Loading && isRefreshing) {
            isRefreshing = false // stop circular indicator
        }
    }

    Scaffold(
        topBar = {
            MarketTrackerHeader {
                Text(
                    text = "Usuários",
                    color = Color.White,
                    fontFamily = mainFont,
                    fontSize = 30.sp,
                    modifier = Modifier
                        .align(alignment = Alignment.Center)
                )

                Box(
                    modifier = Modifier.align(alignment = Alignment.CenterEnd)
                ) {
                    MarketTrackerBackButton(onBackRequested = onBackRequested)
                }
            }
        }
    ) { paddingValues ->
        PullToRefreshLazyColumn(
            isRefreshing = isRefreshing,
            onRefresh = {
                isRefreshing = true
                onFetchUsersRequested()
            },
            modifier = Modifier
                .padding(paddingValues)
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize(),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                UsersListView(
                    state = state,
                    query = query,
                    isActive = state !is UsersScreenState.Loading,
                    onQueryChangeRequested = onQueryChangeRequested,
                    onFetchUsersRequested = onFetchUsersRequested,
                    onFetchMoreUsersRequested = onFetchMoreUsersRequested,
                    onAddUserToList = onAddUserToListRequested,
                )
            }
        }
    }
}