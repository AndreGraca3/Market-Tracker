package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Euro
import androidx.compose.material3.Badge
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.price.Price
import pt.isel.markettracker.domain.model.market.price.Promotion
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.centToEuro
import java.time.LocalDateTime

@Composable
fun PriceLabel(price: Price, modifier: Modifier = Modifier) {
    Row(
        modifier = modifier,
        verticalAlignment = Alignment.CenterVertically
    ) {
        Icon(
            Icons.Filled.Euro,
            contentDescription = null,
        )

        Text(
            text = price.finalPrice.centToEuro(),
            style = MarketTrackerTypography.titleLarge,
            maxLines = 1,
            modifier = Modifier.padding(end = 4.dp)
        )

        if (price.promotion != null) {
            Box(
                contentAlignment = Alignment.TopStart,
                modifier = Modifier
                    .align(Alignment.Top)
                    .clip(RoundedCornerShape(40.dp, 0.dp, 0.dp, 40.dp))
                    .background(Color.Red)
            ) {
                Text(
                    text = "-${price.promotion.percentage}%",
                    style = MarketTrackerTypography.bodySmall,
                    modifier = Modifier.padding(horizontal = 2.dp),
                    maxLines = 1,
                )
            }
        }

        /*
        BadgedBox(badge = {
            if (price.promotion != null) {
                Badge(
                    containerColor = Color.Red,
                    modifier = Modifier
                        .clip(RoundedCornerShape(50.dp, 0.dp, 0.dp, 20.dp))
                ) {
                    Text(
                        text = "-${price.promotion.percentage}%",
                        style = MarketTrackerTypography.bodySmall,
                        maxLines = 1,
                    )
                }
            }
        }) {
            Text(
                text = price.finalPrice.centToEuro(),
                style = MarketTrackerTypography.titleLarge,
                maxLines = 1,
            )
        }*/
    }
}

@Preview
@Composable
fun PriceLabelPreview() {
    PriceLabel(
        Price(
            1000,
            30,
            Promotion(10, LocalDateTime.now()), LocalDateTime.now()
        )
    )
}