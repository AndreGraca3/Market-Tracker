package pt.isel.markettracker.ui.screens.product.specs

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.Product
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.ui.theme.Primary600
import pt.isel.markettracker.utils.shimmerEffect

@Composable
fun ProductSpecs(product: Product?) {
    Column(
        verticalArrangement = Arrangement.spacedBy(4.dp),
    ) {
        if (product == null) {
            repeat(3) {
                Box(
                    modifier = Modifier
                        .clip(RoundedCornerShape(8.dp))
                        .fillMaxWidth(if (it == 0) 0.3F else 1F)
                        .height((22).dp)
                        .shimmerEffect()
                )
            }
        } else {
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(6.dp),
            ) {
                Text(
                    text = product.category.name,
                    style = MarketTrackerTypography.bodyMedium,
                    fontWeight = FontWeight.Bold,
                    color = Color.Gray
                )

                Text(
                    text = "•",
                    style = MarketTrackerTypography.bodyMedium,
                    fontWeight = FontWeight.Bold
                )

                Text(
                    text = product.brand.name,
                    style = MarketTrackerTypography.bodyMedium,
                    fontWeight = FontWeight.Bold,
                    color = Primary600,
                    maxLines = 1,
                    overflow = TextOverflow.Ellipsis
                )
            }
            Text(
                text = product.name,
                style = MarketTrackerTypography.titleLarge,
                fontWeight = FontWeight.Bold
            )

            Text(
                text = "${product.quantity} ${product.unit.title}",
                style = MarketTrackerTypography.bodyMedium,
                fontWeight = FontWeight.Bold,
                color = Color.Gray,
                modifier = Modifier.padding(top = 4.dp)
            )
        }
    }
}