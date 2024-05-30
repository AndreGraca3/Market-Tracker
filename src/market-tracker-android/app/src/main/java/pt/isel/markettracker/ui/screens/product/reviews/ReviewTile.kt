package pt.isel.markettracker.ui.screens.product.reviews

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Person
import androidx.compose.material.icons.filled.Star
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.AsyncImage
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.domain.model.market.inventory.product.ProductReview
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.utils.timeSince
import java.time.LocalDateTime

@Composable
fun ReviewTile(review: ProductReview) {

    val avatarSize = 28.dp

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
            review.client.avatar?.let {
                AsyncImage(
                    model = review.client.avatar, contentDescription = "avatar",
                    modifier = Modifier
                        .size(avatarSize)
                        .clip(CircleShape)
                        .border(1.dp, Color.Black, CircleShape)
                )
            } ?: Icon(
                imageVector = Icons.Default.Person,
                contentDescription = "Default Avatar",
                modifier = Modifier
                    .size(avatarSize)
                    .clip(CircleShape)
                    .border(1.dp, Color.Black, CircleShape)
            )
        }

        Column(
            verticalArrangement = Arrangement.spacedBy(4.dp)
        ) {
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(4.dp)
            ) {
                Text(
                    text = review.client.username,
                    style = MarketTrackerTypography.bodyLarge,
                    fontWeight = FontWeight.Bold
                )

                Text(
                    text = "• há ${timeSince(review.createdAt)}",
                    style = MarketTrackerTypography.bodySmall,
                )
            }

            Row {
                repeat(review.rating) {
                    Icon(
                        imageVector = Icons.Default.Star,
                        contentDescription = "Star Icon"
                    )
                }
            }

            review.comment?.let {
                Text(
                    text = review.comment,
                    style = MarketTrackerTypography.bodyMedium
                )
            }
        }
    }
}

@Preview
@Composable
fun ReviewTilePreview() {
    MarkettrackerTheme {
        ReviewTile(
            ProductReview(
                id = 1,
                productId = "84312332",
                client = ClientItem("username", "username", "avatar"),
                rating = 5,
                comment = "This is a review",
                createdAt = LocalDateTime.now()
            )
        )
    }
}