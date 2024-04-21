package pt.isel.markettracker.ui.screens.product.stores

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.screens.products.card.PriceLabel
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun StoreTile(
    storeName: String,
    storeAddress: String,
    storeCity: String,
    storePrice: Int,
    onStoreSelected: () -> Unit
) {
    Row(
        verticalAlignment = Alignment.CenterVertically,
        modifier = Modifier.clickable { onStoreSelected() }
    ) {
        Column(
            modifier = Modifier
                .fillMaxWidth(0.7F),
            verticalArrangement = Arrangement.spacedBy(10.dp),
            horizontalAlignment = Alignment.Start
        ) {
            Text(text = storeName, style = MarketTrackerTypography.bodyLarge)
            Text(text = storeAddress, style = MarketTrackerTypography.bodyMedium)
            Text(text = storeCity, style = MarketTrackerTypography.bodyMedium)
        }

        PriceLabel(price = storePrice)
    }
}