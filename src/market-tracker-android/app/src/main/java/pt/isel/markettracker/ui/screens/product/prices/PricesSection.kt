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
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.Fail
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Loaded
import pt.isel.markettracker.domain.extractValue
import pt.isel.markettracker.http.models.price.CompanyPrices
import pt.isel.markettracker.ui.theme.MarketTrackerTypography
import pt.isel.markettracker.utils.shimmerEffect

@Composable
fun PricesSection(pricesState: IOState<List<CompanyPrices>>) {
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
            modifier = Modifier
                .fillMaxWidth()
        ) {
            when (pricesState) {
                is Loaded -> {
                    val companiesPrices = pricesState.extractValue()
                    if (companiesPrices.isEmpty()) {
                        Text(
                            text = stringResource(R.string.no_prices_found),
                            style = MarketTrackerTypography.bodyMedium,
                            color = Color.Red
                        )
                    } else companiesPrices.forEach {
                        CompanyRow(companyPrices = it)
                        HorizontalDivider()
                    }
                }

                is Fail -> {
                    Text(
                        text = pricesState.exception.message
                            ?: stringResource(id = R.string.prices_loading_error),
                        style = MarketTrackerTypography.bodyMedium
                    )
                }

                else -> {
                    (1..3).forEach {
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
    }
}