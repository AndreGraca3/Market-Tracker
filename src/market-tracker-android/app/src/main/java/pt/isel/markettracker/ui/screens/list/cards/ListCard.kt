package pt.isel.markettracker.ui.screens.list.cards

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.combinedClickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
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
import pt.isel.markettracker.ui.screens.list.components.ListNameDisplay
import pt.isel.markettracker.ui.screens.list.components.ListStatusIcons
import pt.isel.markettracker.ui.theme.Primary900
import pt.isel.markettracker.utils.advanceShadow

@OptIn(ExperimentalFoundationApi::class)
@Composable
fun ListCard(
    listInfo: ShoppingList,
    onListItemClick: (String) -> Unit,
    onLongClickRequest: (String) -> Unit,
) {
    val shape = RoundedCornerShape(8.dp)

    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier
            .size(width = 350.dp, 125.dp)
            .border(2.dp, Color.Cyan)
    ) {
        Card(
            modifier = Modifier
                .fillMaxSize()
                .clip(shape)
                .clickable { }
                .combinedClickable(
                    onClick = { onListItemClick(listInfo.id) },
                    onLongClick = { onLongClickRequest(listInfo.id) }
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
                Row {
                    Column {
                        ListNameDisplay(
                            listInfo.name,
                            modifier = Modifier
                                .fillMaxHeight()
                                .fillMaxWidth(0.7F)
                        )
                    }

                    Column {
                        ListStatusIcons(
                            isOwner = listInfo.isOwner,
                            numberOfParticipants = 15,
                            modifier = Modifier
                                .fillMaxSize()
                        )
                    }
                }
            }
        }
    }
}
