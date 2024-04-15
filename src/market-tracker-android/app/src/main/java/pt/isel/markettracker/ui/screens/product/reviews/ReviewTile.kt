package pt.isel.markettracker.ui.screens.product.reviews

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Star
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.ui.theme.MarkettrackerTheme

@Composable
fun ReviewTile() {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(4.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(8.dp)
    ) {
        Box(
            modifier = Modifier
                .align(Alignment.Top),
            contentAlignment = Alignment.TopCenter
        ) {
            Image(
                painter = painterResource(id = R.drawable.user_icon),
                contentDescription = "Review",
                modifier = Modifier
                    .size(26.dp)
                    .clip(CircleShape)
            )
        }
        Column(
            verticalArrangement = Arrangement.spacedBy(2.dp)
        ) {
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(4.dp)
            ) {
                Text(
                    text = "User André Graça",
                    style = MarketTrackerTypography.bodyLarge,
                    fontWeight = FontWeight.Bold
                )

                Text(
                    text = "• há 2 dias",
                    style = MarketTrackerTypography.bodySmall,
                )
            }
            Row {
                (1..5).forEach {
                    Icon(
                        imageVector = Icons.Default.Star,
                        contentDescription = "Star Icon"
                    )
                }
            }
            Text(
                text = "Review - Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                style = MarketTrackerTypography.bodyMedium
            )
        }
    }
}

@Preview
@Composable
fun ReviewTilePreview() {
    MarkettrackerTheme {
        ReviewTile()
    }
}