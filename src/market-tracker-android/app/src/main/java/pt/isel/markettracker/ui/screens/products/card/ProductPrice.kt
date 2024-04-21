package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.width
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun ProductPrice(price: Int) {
    Row(
        verticalAlignment = Alignment.CenterVertically
    ) {
        Text(
            text = "Desde",
            style = MarketTrackerTypography.bodyMedium
        )

        Spacer(modifier = Modifier.width(6.dp))

        PriceLabel(price)
    }
}