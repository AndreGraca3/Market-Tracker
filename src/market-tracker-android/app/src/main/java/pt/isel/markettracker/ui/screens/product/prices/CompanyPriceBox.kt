package pt.isel.markettracker.ui.screens.product.prices

import android.util.Log
import androidx.compose.foundation.background
import androidx.compose.foundation.gestures.detectTapGestures
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.RowScope
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Error
import androidx.compose.material3.Icon
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.ui.components.button.AddToListButton
import pt.isel.markettracker.ui.screens.products.card.PriceLabel
import pt.isel.markettracker.ui.theme.Grey

@Composable
fun RowScope.CompanyPriceBox(price: Int) {
    Column(
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(2.dp),
        modifier = Modifier
            .align(Alignment.Bottom)
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(4.dp)
        ) {
            PriceLabel(price)
            Icon(
                imageVector = Icons.Default.Error,
                contentDescription = "Price last checked",
                modifier = Modifier
                    .pointerInput(Unit) {
                        detectTapGestures(
                            onLongPress = {
                                Log.v("CompanyPriceBox", "Price last checked clicked")
                            }
                        )
                    }
                    .background(Grey, shape = CircleShape)
            )
        }
        AddToListButton(onClick = {})
    }

}