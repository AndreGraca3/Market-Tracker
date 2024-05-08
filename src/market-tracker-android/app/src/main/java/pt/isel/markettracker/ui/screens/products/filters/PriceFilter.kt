package pt.isel.markettracker.ui.screens.products.filters

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.components.sliders.IntRangeSlider
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun PriceFilter(
    minPrice: Int,
    maxPrice: Int,
    onValueChangeFinished: (IntRange) -> Unit,
    enabled: Boolean
) {
    Column(
        verticalArrangement = Arrangement.spacedBy(8.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        var sliderPosition by remember {
            mutableStateOf(minPrice.toFloat()..maxPrice.toFloat())
        }

        Text(
            text = "Price Range",
            textAlign = TextAlign.Start,
            modifier = Modifier.fillMaxWidth()
        )

        HorizontalDivider(modifier = Modifier.fillMaxWidth())

        IntRangeSlider(
            sliderPosition = sliderPosition,
            onValueChange = {
                sliderPosition = it
            },
            onValueChangeFinished = onValueChangeFinished,
            enabled = enabled
        )

        Text(
            text = "${sliderPosition.start.toInt()}€ - ${sliderPosition.endInclusive.toInt()}€",
            style = MarketTrackerTypography.labelMedium
        )
    }
}