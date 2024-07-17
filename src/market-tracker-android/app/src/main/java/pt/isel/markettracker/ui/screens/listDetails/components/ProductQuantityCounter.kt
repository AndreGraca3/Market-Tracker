package pt.isel.markettracker.ui.screens.listDetails.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Remove
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.components.buttons.MarketTrackerOutlinedButton
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun ProductQuantityCounter(
    quantity: Int,
    enabled: Boolean,
    onQuantityIncreaseRequest: () -> Unit,
    onQuantityDecreaseRequest: () -> Unit,
) {
    Row(
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(6.dp, Alignment.CenterHorizontally),
    ) {
        MarketTrackerOutlinedButton(
            modifier = Modifier.weight(1F),
            icon = Icons.Default.Remove,
            enabled = enabled,
            onClick = onQuantityDecreaseRequest
        )

        Text(
            modifier = Modifier.weight(1F),
            text = "$quantity",
            fontFamily = mainFont,
            textAlign = TextAlign.Center,
        )

        MarketTrackerOutlinedButton(
            modifier = Modifier.weight(1F),
            icon = Icons.Default.Add,
            enabled = enabled,
            onClick = onQuantityIncreaseRequest
        )

    }
}