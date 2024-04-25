package pt.isel.markettracker.ui.components.icons

import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.StarHalf
import androidx.compose.material.icons.filled.Star
import androidx.compose.material.icons.filled.StarBorder
import androidx.compose.material3.Icon
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import pt.isel.markettracker.ui.theme.Primary500

@Composable
fun StarIcon(filled: Boolean?, modifier: Modifier = Modifier) {
    Icon(
        imageVector = when (filled) {
            true -> Icons.Filled.Star
            false -> Icons.AutoMirrored.Filled.StarHalf
            null -> Icons.Filled.StarBorder
        },
        contentDescription = "Star",
        modifier = modifier,
        tint = Primary500
    )
}