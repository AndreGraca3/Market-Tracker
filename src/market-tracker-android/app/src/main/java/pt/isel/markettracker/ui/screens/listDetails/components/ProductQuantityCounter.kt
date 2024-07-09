package pt.isel.markettracker.ui.screens.listDetails.components

import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Remove
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clipToBounds
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.Primary400

@Composable
fun ProductQuantityCounter(
    quantity: Int,
    onQuantityIncreaseRequest: () -> Unit,
    onQuantityDecreaseRequest: () -> Unit
) {
    Row (
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.Center,
        modifier = Modifier.fillMaxSize()
    ) {
        OutlinedButton(
            onClick = onQuantityIncreaseRequest,
            colors = ButtonDefaults.buttonColors(Primary400),
            border = BorderStroke(2.dp, Color.Black),
            modifier = Modifier
                .fillMaxSize()
                .clipToBounds()
                .weight(1F)
        ) {
            Row(
                modifier = Modifier.fillMaxSize(),
                horizontalArrangement = Arrangement.Center,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Icon(
                    imageVector = Icons.Filled.Add,
                    contentDescription = null,
                    tint = Color.White,
                )
            }
        }

        Text("$quantity")

        OutlinedButton(
            onClick = onQuantityDecreaseRequest,
            colors = ButtonDefaults.buttonColors(Primary400),
            border = BorderStroke(2.dp, Color.Black),
            modifier = Modifier
                .fillMaxSize()
                .clipToBounds()
                .weight(1F)
        ) {
            Row(
                modifier = Modifier.fillMaxSize(),
                horizontalArrangement = Arrangement.Center,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Icon(
                    imageVector = Icons.Filled.Remove,
                    contentDescription = null,
                    tint = Color.White,
                )
            }
        }
    }
}