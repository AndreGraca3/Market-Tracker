package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.style.TextDecoration
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.price.Price
import pt.isel.markettracker.domain.model.market.price.Promotion
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.centToEuro
import java.time.LocalDateTime

@Composable
fun PriceLabel(price: Price, showPromotion: Boolean, modifier: Modifier = Modifier) {
    Column(
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(2.dp),
        modifier = modifier
    ) {
        Text(
            text = "${price.finalPrice.centToEuro()} €",
            style = MarketTrackerTypography.titleLarge,
            maxLines = 1,
        )

        if (showPromotion && price.promotion != null) {
            Text(
                text = "${price.basePrice.centToEuro()} €",
                style = TextStyle(textDecoration = TextDecoration.LineThrough),
                color = Color.Red
            )
        }
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
        ),
        true
    )
}