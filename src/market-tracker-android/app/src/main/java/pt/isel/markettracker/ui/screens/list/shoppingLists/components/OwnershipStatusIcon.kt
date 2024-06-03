package pt.isel.markettracker.ui.screens.list.shoppingLists.components

import androidx.compose.foundation.Canvas
import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Icon
import androidx.compose.ui.Alignment
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.drawscope.inset
import androidx.compose.ui.res.painterResource
import com.example.markettracker.R
import java.time.LocalDateTime

@Composable
fun OwnershipStatusIcon(isOwner: Boolean, archivedAt: LocalDateTime?) {
    if (isOwner) {
        Image(
            painter = painterResource(id = R.drawable.crown),
            contentDescription = "",
            modifier = Modifier
                .size(24.dp)
                .padding(end = 6.dp, top = 2.dp)
        )
    }
    Box(modifier = Modifier.size(32.dp)) {
        Canvas(modifier = Modifier.matchParentSize()) {
            val circleColor = if (archivedAt == null) Color.Green else Color.Red
            inset(5.dp.toPx(), 5.dp.toPx()) {
                drawCircle(color = circleColor, radius = size.minDimension / 2 + 8)
            }
        }
        Icon(
            painter = painterResource(id = if (archivedAt == null) R.drawable.unlock else R.drawable.lock),
            contentDescription = "",
            modifier = Modifier
                .size(22.dp)
                .align(Alignment.Center)
        )
    }
}
