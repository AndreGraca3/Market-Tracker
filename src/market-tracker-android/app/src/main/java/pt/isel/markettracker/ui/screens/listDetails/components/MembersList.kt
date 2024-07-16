package pt.isel.markettracker.ui.screens.listDetails.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.ui.screens.listDetails.cards.UserCard

@Composable
fun MembersList(
    users: List<ClientItem>,
    ownerId: String,
    onRemoveUserFromLisTRequested: (String) -> Unit,
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
                isOwner = users[it].id == ownerId,
                userToListRequested = onRemoveUserFromLisTRequested
            )
        }
    }
}