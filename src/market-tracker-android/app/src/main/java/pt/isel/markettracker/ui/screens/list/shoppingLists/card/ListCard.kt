package pt.isel.markettracker.ui.screens.list.shoppingLists.card

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.combinedClickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.domain.model.list.ShoppingListSocial
import pt.isel.markettracker.ui.screens.list.shoppingLists.components.ListNameDisplay
import pt.isel.markettracker.ui.screens.list.shoppingLists.components.OwnershipStatusIcon
import pt.isel.markettracker.ui.screens.list.shoppingLists.components.ParticipantBadge
import pt.isel.markettracker.ui.theme.Primary900
import pt.isel.markettracker.utils.advanceShadow

@OptIn(ExperimentalFoundationApi::class)
@Composable
fun ListItemCard(
    listInfo: ShoppingList,
    onListItemClick: (String) -> Unit,
    onLongClickRequest: () -> Unit
) {
    val shape = RoundedCornerShape(8.dp)

    Card(
        modifier = Modifier
            .fillMaxSize()
            .clip(shape)
            .clickable { }
            .combinedClickable(
                onClick = { onListItemClick(listInfo.id) },
                onLongClick = onLongClickRequest
            )
            .padding(2.dp)
            .border(2.dp, Color.Black.copy(.6F), shape)
            .advanceShadow(Primary900, blurRadius = 24.dp),
        colors = CardDefaults.cardColors(Color.White)
    ) {
        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(14.dp, 8.dp)
        ) {
            Row(
                modifier = Modifier
                    .fillMaxSize(),
                verticalAlignment = Alignment.CenterVertically
            ) {
                Spacer(modifier = Modifier.width(2.dp))
                Column(
                    modifier = Modifier
                        .weight(0.4f)
                ) {
                    ListNameDisplay(listInfo.name)
                }
                Column(
                    modifier = Modifier
                        .weight(0.3f)
                ) {
                    ParticipantBadge(4)
                }
            }
            Row(
                modifier = Modifier
                    .align(Alignment.TopEnd)
                    .padding(start = 2.dp, top = 4.dp)
            ) {
                OwnershipStatusIcon(true, listInfo.archivedAt)
            }
        }
    }
}


