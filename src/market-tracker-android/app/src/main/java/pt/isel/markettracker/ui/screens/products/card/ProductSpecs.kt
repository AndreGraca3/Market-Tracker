package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun ProductSpecs(product: ProductInfo) {

    Column(
        verticalArrangement = Arrangement.spacedBy(4.dp),
    ) {
        Text(
            text = product.name,
            style = MarketTrackerTypography.titleMedium,
            overflow = TextOverflow.Ellipsis,
            maxLines = 1,
        )
        Text(
            text = product.brand,
            style = MarketTrackerTypography.labelSmall,
            overflow = TextOverflow.Ellipsis,
            maxLines = 1,
            textAlign = TextAlign.Left
        )
        Text(
            text = product.category,
            style = MarketTrackerTypography.labelMedium,
            overflow = TextOverflow.Ellipsis,
            maxLines = 1,
            textAlign = TextAlign.Left
        )
    }

}