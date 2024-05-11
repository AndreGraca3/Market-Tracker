package pt.isel.markettracker.ui.screens.list

import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.list.ListInfo
import pt.isel.markettracker.ui.theme.Primary900
import pt.isel.markettracker.utils.advanceShadow

@Composable
fun ListItemCard(listItem: ListInfo, onListItemClick: (Int) -> Unit) {
    val shape = RoundedCornerShape(8.dp)
    Card(
        modifier = Modifier
            .fillMaxSize()
            .clip(shape)
            .clickable { onListItemClick(listItem.id) }
            .padding(2.dp)
            .border(2.dp, Color.Black.copy(.6F), shape)
            .advanceShadow(Primary900, blurRadius = 24.dp),
        colors = CardDefaults.cardColors(Color.White)
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(10.dp),
            modifier = Modifier
                .fillMaxSize()
                .padding(14.dp, 8.dp)
        ) {
            Text(text = listItem.listName)
            Text(text = listItem.archivedAt.toString())
        }
    }
}