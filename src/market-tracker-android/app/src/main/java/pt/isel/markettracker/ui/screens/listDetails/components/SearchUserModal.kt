package pt.isel.markettracker.ui.screens.listDetails.components

import android.util.Log
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.PersonAdd
import androidx.compose.material.icons.filled.PersonOff
import androidx.compose.material.icons.filled.PersonRemove
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ModalDrawerSheet
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.RectangleShape
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.listDetails.ListDetailsScreenState
import pt.isel.markettracker.ui.screens.listDetails.cards.UserCard
import pt.isel.markettracker.ui.screens.listDetails.extractSearchResult
import pt.isel.markettracker.ui.screens.listDetails.extractShoppingListSocial
import pt.isel.markettracker.ui.theme.Primary600

@Composable
fun SearchUserModal(
    state: ListDetailsScreenState,
    query: String,
    onQueryChange: (String) -> Unit,
    onSearch: (String) -> Unit,
    onAddUserToLisTRequested: (String) -> Unit,
    onRemoveUserFromLisTRequested: (String) -> Unit,
    modifier: Modifier = Modifier,
) {
    ModalDrawerSheet(
        modifier = modifier
            .fillMaxHeight()
            .fillMaxWidth(0.7F)
            .border(2.dp, color = Color.Black),
        drawerShape = RectangleShape,
        drawerContainerColor = Primary600,
        content = {
            Column {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    when (state) {
                        is ListDetailsScreenState.Loading -> {
                            LoadingIcon()
                        }

                        is ListDetailsScreenState.Failed -> {
                            Text(
                                text = stringResource(id = R.string.failed_to_fetch_users),
                                color = Color.Gray,
                                fontWeight = FontWeight.Bold
                            )
                        }

                        is ListDetailsScreenState.Loaded,
                        is ListDetailsScreenState.SearchingUsers,
                        -> {
                            Log.v("List", "State is $state")
                            val ola = state.extractShoppingListSocial()
                            val ownerId = state.extractShoppingListSocial()?.owner?.id
                            val users = state.extractShoppingListSocial()?.members ?: emptyList()
                            val searchResult = state.extractSearchResult()
                            Log.v("List", "ShoppingListSocial is $ola")
                            Log.v("List", "users is $users")
                            Log.v("List", "searchResult is $searchResult")
                            Column {
                                Box(
                                    modifier = Modifier
                                        .fillMaxWidth()
                                        .fillMaxHeight(0.4F),
                                    contentAlignment = Alignment.TopCenter
                                ) {
                                    LazyColumn(
                                        contentPadding = PaddingValues(10.dp),
                                        verticalArrangement = Arrangement.spacedBy(
                                            10.dp,
                                            Alignment.Top
                                        )
                                    ) {
                                        items(users.size) {
                                            UserCard(
                                                user = users[it],
                                                icon = if (users[it].id == ownerId) Icons.Default.PersonOff else Icons.Default.PersonRemove,
                                                userToListRequested = onRemoveUserFromLisTRequested
                                            )
                                        }
                                    }
                                }

                                Box(
                                    modifier = Modifier
                                        .fillMaxWidth()
                                        .fillMaxHeight(0.2f),
                                ) {

                                    UserSearchBar(
                                        query = query,
                                        onQueryChange = onQueryChange,
                                        onSearch = onSearch,
                                        active = state is ListDetailsScreenState.Loaded,
                                        modifier = Modifier.padding(horizontal = 2.dp),
                                    )
                                }

                                Box(
                                    modifier = Modifier.fillMaxSize(),
                                    contentAlignment = Alignment.TopCenter
                                ) {
                                    if (state is ListDetailsScreenState.SearchingUsers) {
                                        LoadingIcon(text = stringResource(R.string.searching_for_users))
                                    }
                                    if (searchResult != null) {
                                        LazyColumn(
                                            contentPadding = PaddingValues(10.dp),
                                            verticalArrangement = Arrangement.spacedBy(
                                                10.dp,
                                                Alignment.Top
                                            )
                                        ) {
                                            items(searchResult.items.size) {
                                                if (searchResult.items[it].id != ownerId) {
                                                    UserCard(
                                                        user = searchResult.items[it],
                                                        icon = Icons.Default.PersonAdd,
                                                        userToListRequested = onAddUserToLisTRequested
                                                    )
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        else -> {
                            Text("Estou no Else :)")
                        }
                    }
                }
            }
        }
    )
}