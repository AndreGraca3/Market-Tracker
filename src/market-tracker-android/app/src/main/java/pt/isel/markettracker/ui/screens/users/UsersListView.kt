package pt.isel.markettracker.ui.screens.users

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.SearchBar
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.users.states.UsersScreenState
import pt.isel.markettracker.ui.screens.users.states.extractHasMore
import pt.isel.markettracker.ui.screens.users.states.extractUsers

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun UsersListView(
    state: UsersScreenState,
    query: String,
    isActive: Boolean,
    onQueryChangeRequested: (String) -> Unit,
    onFetchUsersRequested: () -> Unit,
    onFetchMoreUsersRequested: () -> Unit,
    onAddUserToList: (String) -> Unit,
) {
    val users = state.extractUsers()

    val scrollState = rememberLazyListState()
    val isItemReachEndScroll by remember {
        derivedStateOf {
            scrollState.layoutInfo.visibleItemsInfo.lastOrNull()?.index ==
                    scrollState.layoutInfo.totalItemsCount - 1
        }
    }

    LaunchedEffect(key1 = isItemReachEndScroll) {
        if (isItemReachEndScroll && state.extractHasMore()) {
            onFetchMoreUsersRequested()
        }
    }

    LaunchedEffect(state) {
        if (state is UsersScreenState.Loading) {
            scrollState.scrollToItem(0)
        }
    }

    Box(
        contentAlignment = Alignment.TopCenter,
        modifier = Modifier.fillMaxSize()
    ) {
        SearchBar(
            query = query,
            onQueryChange = onQueryChangeRequested,
            onSearch = {
                onFetchUsersRequested()
            },
            active = isActive,
            onActiveChange = {}
        ) {
            when (state) {
                is UsersScreenState.Loaded -> {
                    UsersList(
                        lazyGridState = scrollState,
                        hasMore = state.hasMore,
                        users = users,
                        onAddUserToList = {
                            onAddUserToList(it)
                        },
                    )
                }

                is UsersScreenState.Failed -> {
                    LazyColumn(
                        modifier = Modifier.fillMaxSize(),
                        verticalArrangement = Arrangement.Center,
                        horizontalAlignment = Alignment.CenterHorizontally
                    ) {
                        item {
                            Image(
                                painter = painterResource(id = R.drawable.server_error),
                                contentDescription = "No users"
                            )
                        }
                    }
                }

                else -> {
                    LoadingIcon(stringResource(id = R.string.searching_for_users))
                }
            }
        }
    }
}