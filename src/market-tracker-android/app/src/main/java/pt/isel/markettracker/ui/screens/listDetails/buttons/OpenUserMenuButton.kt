package pt.isel.markettracker.ui.screens.listDetails.buttons

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R

@Composable
fun OpenUserMenuButton(
    modifier: Modifier = Modifier,
    enabled: Boolean = true,
    onClick: () -> Unit,
) {
    Box(
        modifier = modifier
            .size(52.dp)
    ) {
        IconButton(
            enabled = enabled,
            onClick = onClick
        ) {
            Icon(
                painter = painterResource(R.drawable.person_add),
                contentDescription = "open_user_menu",
                tint = Color.White
            )
        }
    }
}