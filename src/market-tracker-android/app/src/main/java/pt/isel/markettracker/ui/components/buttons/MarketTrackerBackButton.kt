package pt.isel.markettracker.ui.components.buttons

import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import pt.isel.markettracker.R

@Composable
fun MarketTrackerBackButton(
    onBackRequested: () -> Unit,
) {
    IconButton(
        onClick = onBackRequested
    ) {
        Icon(
            painter = painterResource(id = R.drawable.arrow_back),
            contentDescription = "back_button",
            tint = Color.White
        )
    }
}