package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.padding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Euro
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.centToEuro

@Composable
fun PriceLabel(price: Int) {
    Row(
        verticalAlignment = Alignment.CenterVertically
    ) {
        Icon(
            Icons.Filled.Euro,
            contentDescription = null,
        )
        Text(
            text = price.centToEuro(),
            style = MarketTrackerTypography.titleLarge,
            maxLines = 1,
            modifier = Modifier.padding(start = 2.dp)
        )
    }
}

@Preview
@Composable
fun PriceLabelPreview() {
    PriceLabel(100)
}