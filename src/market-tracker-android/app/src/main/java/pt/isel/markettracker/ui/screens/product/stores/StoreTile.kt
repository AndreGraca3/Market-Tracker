package pt.isel.markettracker.ui.screens.product.stores

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.model.market.price.StoreOfferItem
import pt.isel.markettracker.ui.screens.products.card.PriceLabel
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun StoreTile(
    storeOffer: StoreOfferItem,
    onStoreSelected: () -> Unit
) {
    Row(
        verticalAlignment = Alignment.CenterVertically,
        modifier = Modifier
            .fillMaxWidth()
            .clickable { onStoreSelected() }
            .padding(16.dp, 8.dp)
    ) {
        Column(
            modifier = Modifier
                .weight(1F),
            verticalArrangement = Arrangement.spacedBy(8.dp),
            horizontalAlignment = Alignment.Start
        ) {
            Text(
                text = storeOffer.store.name,
                style = MarketTrackerTypography.bodyLarge,
                fontWeight = FontWeight.SemiBold,
                color = Color.DarkGray
            )

            listOf(storeOffer.store.address, storeOffer.store.city?.name).forEach {
                if (it != null) {
                    Text(
                        text = it,
                        style = MarketTrackerTypography.bodyMedium,
                        color = Color.DarkGray
                    )
                }
            }
        }

        Column(
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            PriceLabel(price = storeOffer.price, true)

            if (!storeOffer.isAvailable) {
                Text(
                    text = stringResource(R.string.not_available),
                    style = MarketTrackerTypography.bodyMedium,
                    color = Color.Red
                )
            }
        }
    }
}