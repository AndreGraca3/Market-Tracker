package pt.isel.markettracker.ui.components.icons

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.size
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.StarHalf
import androidx.compose.material.icons.filled.Star
import androidx.compose.material.icons.filled.StarBorder
import androidx.compose.material3.Icon
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.Primary500

@Composable
fun RatingStarsRow(rating: Double, onStarClicked: ((Int) -> Unit)? = null) {
    Row {
        repeat(5) {
            StarIcon(
                filled = when {
                    rating >= it + 1 -> StarType.FILLED
                    rating >= it + 0.5 -> StarType.HALF
                    else -> StarType.EMPTY
                },
                modifier = Modifier
                    .size(32.dp)
                    .then(if (onStarClicked == null) Modifier else Modifier.clickable {
                        onStarClicked(it + 1)
                    })
            )
        }
    }
}

@Composable
fun StarIcon(filled: StarType, modifier: Modifier = Modifier) {
    Icon(
        imageVector = when (filled) {
            StarType.FILLED -> Icons.Filled.Star
            StarType.HALF -> Icons.AutoMirrored.Filled.StarHalf
            StarType.EMPTY -> Icons.Filled.StarBorder
        },
        contentDescription = "Star",
        modifier = modifier,
        tint = Primary500
    )
}

enum class StarType {
    FILLED, HALF, EMPTY
}