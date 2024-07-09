package pt.isel.markettracker.ui.screens.list.buttons

import androidx.compose.foundation.layout.Box
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import pt.isel.markettracker.R

@Composable
fun AddListButton(
    onAddListRequested: () -> Unit,
    enabled: Boolean,
    modifier: Modifier,
) {
    Box(
        modifier = modifier
    ) {
        IconButton(
            enabled = enabled,
            onClick = onAddListRequested
        ) {
            Icon(
                painter = painterResource(if (enabled) R.drawable.add_shopping_cart else R.drawable.shopping_cart_off),
                contentDescription = "settings_icon",
                tint = Color.White
            )
        }
    }
}