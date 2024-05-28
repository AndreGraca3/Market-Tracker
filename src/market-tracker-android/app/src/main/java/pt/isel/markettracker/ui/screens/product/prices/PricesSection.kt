package pt.isel.markettracker.ui.screens.product.prices

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Warning
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.model.market.price.ProductPrices
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.shimmerEffect

@Composable
fun PricesSection(productPrices: ProductPrices?) {
    Column(
        verticalArrangement = Arrangement.spacedBy(18.dp)
    ) {
        Text(
            text = stringResource(id = R.string.prices_section_title),
            fontWeight = FontWeight.SemiBold,
            color = Color.DarkGray,
            style = MarketTrackerTypography.titleLarge
        )

        Row(
            horizontalArrangement = Arrangement.spacedBy(4.dp),
        ) {
            Icon(imageVector = Icons.Default.Warning, contentDescription = "Prices Warning")
            Text(
                text = stringResource(id = R.string.prices_outdated_warning),
                style = MarketTrackerTypography.bodyMedium
            )
        }

        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(16.dp),
            modifier = Modifier.fillMaxWidth()
        ) {
            val companiesPricing = productPrices?.companies

            companiesPricing?.let {
                if (companiesPricing.isEmpty()) {
                    Text(
                        text = stringResource(R.string.no_prices_found),
                        style = MarketTrackerTypography.bodyMedium,
                        color = Color.Red
                    )
                } else companiesPricing.forEach {
                    CompanyRow(companyPrices = it)
                    HorizontalDivider()
                }
            } ?: repeat(3) {
                Box(
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(50.dp)
                        .clip(RoundedCornerShape(12.dp))
                        .shimmerEffect()
                )
            }
        }
    }
}