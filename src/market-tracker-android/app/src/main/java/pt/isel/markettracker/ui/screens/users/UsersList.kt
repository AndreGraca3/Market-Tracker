package pt.isel.markettracker.ui.screens.users

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.LazyListState
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.ui.screens.listDetails.cards.UserCard
import pt.isel.markettracker.ui.screens.products.grid.ScrollToTopButton

@Composable
fun UsersList(
    lazyGridState: LazyListState,
    hasMore: Boolean,
    users: List<ClientItem>,
    onAddUserToList: (String) -> Unit,
) {
    if (users.isEmpty()) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier.verticalScroll(rememberScrollState())
        ) {
            Image(
                painter = painterResource(id = R.drawable.products_not_found),
                contentDescription = "No users found"
            )
            Text(
                text = stringResource(id = R.string.failed_to_fetch_users)
            )
        }
    }

    LazyColumn(
        state = lazyGridState,
        verticalArrangement = Arrangement.spacedBy(10.dp),
        contentPadding = PaddingValues(12.dp)
    ) {
        items(users.size, key = { users[it].id }) { index ->
            UserCard(
                user = users[index],
                resource = R.drawable.person_add,
                userToListRequested = onAddUserToList
            )
        }
        item {
            if (hasMore) {
                Box(
                    contentAlignment = Alignment.Center,
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(30.dp)
                ) {
                    CircularProgressIndicator(
                        modifier = Modifier
                            .fillMaxHeight()
                            .padding(4.dp)
                    )
                }
            }
        }
    }

    val scope = rememberCoroutineScope()
    val showButton by remember {
        derivedStateOf {
            lazyGridState.firstVisibleItemIndex > 0
        }
    }

    ScrollToTopButton(
        visible = showButton,
        onClick = {
            scope.launch {
                lazyGridState.animateScrollToItem(0)
            }
        }
    )
}