package pt.isel.markettracker.ui.screens.listDetails.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.ui.screens.listDetails.cards.UserCard
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun MembersList(
    users: List<ClientItem>,
    ownerId: String,
    onRemoveUserFromLisTRequested: (String) -> Unit,
    modifier: Modifier = Modifier,
) {
    Box(
        modifier = modifier,
        contentAlignment = Alignment.TopCenter
    ) {
        Box {
            Text(
                modifier = Modifier.align(Alignment.Center),
                text = "Membros",
                textAlign = TextAlign.Center,
                fontFamily = mainFont,
                color = Color.White
            )
        }

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
                    resource = R.drawable.person_remove,
                    userToListRequested = onRemoveUserFromLisTRequested
                )
            }
        }
    }
}