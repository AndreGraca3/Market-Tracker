package pt.isel.markettracker.ui.screens.list

import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
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
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.size
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextOverflow
import com.example.markettracker.R

@Composable
fun ListItemCard(listInfo: ListInfo, onListItemClick: (Int) -> Unit) {
    val shape = RoundedCornerShape(8.dp)

    Card(
        modifier = Modifier
            .fillMaxSize()
            .clip(shape)
            .clickable { onListItemClick(listInfo.id) }
            .padding(2.dp)
            .border(2.dp, Color.Black.copy(.6F), shape)
            .advanceShadow(Primary900, blurRadius = 24.dp),
        colors = CardDefaults.cardColors(Color.White)
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(20.dp),
            modifier = Modifier
                .fillMaxSize()
                .padding(14.dp, 8.dp)
        ) {
            Row (
                modifier = Modifier.fillMaxSize(),
                verticalAlignment = Alignment.CenterVertically
            ) {
                Column(
                    modifier = Modifier.weight(0.8f)
                ) {
                    Text(
                        text = listInfo.listName,
                        maxLines = 2,
                        overflow = TextOverflow.Ellipsis
                    )
                }
                Column(
                    modifier = Modifier.weight(0.2f)
                ) {
                    Image(
                        painter = painterResource(id = when (listInfo.numberOfParticipants) {
                            0 -> R.drawable.group0
                            1 -> R.drawable.group1
                            2 -> R.drawable.group2
                            3 -> R.drawable.group3
                            4 -> R.drawable.group4
                            else -> R.drawable.group4
                        }),
                        contentDescription = "",
                        modifier = Modifier
                            .size(48.dp)
                    )
                }
                Column(
                    modifier = Modifier
                        .weight(0.2f)
                        .padding(start = 8.dp)
                ) {
                    Image(
                        painter = painterResource(id = if (listInfo.archivedAt == null) R.drawable.unlock else R.drawable.lock),
                        contentDescription = "",
                        modifier = Modifier
                            .size(32.dp)
                    )
                }
            }
        }
    }
}