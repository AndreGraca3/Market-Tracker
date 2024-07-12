package pt.isel.markettracker.ui.screens.listDetails.components

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Remove
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import pt.isel.markettracker.ui.components.buttons.MarketTrackerOutlinedButton
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun ProductQuantityCounter(
    quantity: Int,
    enabled: Boolean,
    onQuantityIncreaseRequest: () -> Unit,
    onQuantityDecreaseRequest: () -> Unit,
) {
    Row {
        Box(
            modifier = Modifier
                .fillMaxHeight()
                .weight(0.3F),
            contentAlignment = Alignment.CenterStart
        ) {
            MarketTrackerOutlinedButton(
                icon = Icons.Default.Add,
                enabled = enabled,
                onClick = onQuantityIncreaseRequest
            )
        }

        Box(
            modifier = Modifier
                .fillMaxHeight()
                .fillMaxWidth(0.4F),
            contentAlignment = Alignment.Center
        ) {
            Text(
                text = "$quantity",
                fontFamily = mainFont
            )
        }

        Box(
            modifier = Modifier
                .fillMaxHeight()
                .fillMaxWidth()
                .weight(0.3F),
            contentAlignment = Alignment.CenterEnd
        ) {
            MarketTrackerOutlinedButton(
                icon = Icons.Default.Remove,
                enabled = enabled,
                onClick = onQuantityDecreaseRequest
            )
        }
    }
}