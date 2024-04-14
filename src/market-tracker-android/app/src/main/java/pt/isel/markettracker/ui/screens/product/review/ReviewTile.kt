package pt.isel.markettracker.ui.screens.product.review

import androidx.compose.foundation.Image
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun ReviewTile() {
    Row(
        modifier = Modifier
            .fillMaxWidth().border(1.dp, Color.Red),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(8.dp)
    ) {
        Column(
            modifier = Modifier.fillMaxHeight().border(1.dp, Color.Green),
            verticalArrangement = Arrangement.Top,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Image(
                painter = painterResource(id = R.drawable.mt_logo),
                contentDescription = "Review",
                modifier = Modifier
                    .size(28.dp)
                    .clip(CircleShape)
            )
        }
        Column {
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(4.dp)
            ) {
                Text(
                    text = "User André Graça",
                    style = MarketTrackerTypography.labelMedium
                )

                Text(
                    text = "• há 2 dias",
                    style = MarketTrackerTypography.bodySmall
                )
            }
            Text(
                text = "Review - Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                style = MarketTrackerTypography.bodyMedium
            )
        }
    }
}