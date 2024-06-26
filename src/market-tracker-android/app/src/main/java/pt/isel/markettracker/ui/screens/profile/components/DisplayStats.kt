package pt.isel.markettracker.ui.screens.profile.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.AddAlert
import androidx.compose.material.icons.filled.Favorite
import androidx.compose.material.icons.filled.ShoppingCart
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun DisplayStats(
    nLists: Int,
    nFavorites: Int,
    nAlerts: Int,
) {
    Row(
        horizontalArrangement = Arrangement.Center,
        verticalAlignment = Alignment.CenterVertically,
        modifier = Modifier.fillMaxWidth()
    ) {
        DisplayIcon(
            image = Icons.Default.ShoppingCart,
            value = nLists
        )

        DisplayIcon(
            image = Icons.Default.Favorite,
            value = nFavorites
        )

        DisplayIcon(
            image = Icons.Default.AddAlert,
            value = nAlerts
        )
    }
}

@Composable
fun DisplayIcon(
    image: ImageVector,
    value: Int,
) {
    Box(
        modifier = Modifier.padding(10.dp),
        contentAlignment = Alignment.Center
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.Center
        ) {
            Icon(
                imageVector = image,
                contentDescription = null,
                tint = Color.Black
            )

            Text(
                text = if ("$value".length >= 3) "99+" else "$value",
                textAlign = TextAlign.Center,
                fontFamily = mainFont,
                color = Color.Black,
            )
        }
    }
}