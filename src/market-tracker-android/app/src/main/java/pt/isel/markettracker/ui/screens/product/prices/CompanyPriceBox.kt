package pt.isel.markettracker.ui.screens.product.prices

import androidx.compose.foundation.gestures.detectTapGestures
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.RowScope
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Text
import androidx.compose.material3.TooltipBox
import androidx.compose.material3.TooltipDefaults.rememberPlainTooltipPositionProvider
import androidx.compose.material3.rememberTooltipState
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import pt.isel.markettracker.ui.components.buttons.AddToListButton
import pt.isel.markettracker.ui.screens.products.card.PriceLabel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun RowScope.CompanyPriceBox(price: Int) {
    val positionProvider = rememberPlainTooltipPositionProvider()
    val tooltipState = rememberTooltipState()
    val scope = rememberCoroutineScope()

    Column(
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(4.dp),
        modifier = Modifier
            .align(Alignment.Bottom)
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(4.dp)
        ) {
            TooltipBox(
                positionProvider = positionProvider,
                tooltip = {
                    Text("há 2 horas")
                },
                state = tooltipState
            ) {
                Box(
                    modifier = Modifier.pointerInput(Unit) {
                        detectTapGestures(
                            onLongPress = {
                                scope.launch {
                                    tooltipState.show()
                                }
                            }
                        )
                    }
                ) {
                    PriceLabel(price)
                }
            }
        }
        AddToListButton(onClick = {})
    }

}