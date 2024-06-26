package pt.isel.markettracker.ui.screens.product.components

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBackIosNew
import androidx.compose.material.icons.filled.Favorite
import androidx.compose.material.icons.filled.FavoriteBorder
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Surface
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.ProductPreferences
import pt.isel.markettracker.ui.theme.Grey

@Composable
fun ProductTopBar(
    onBackRequest: () -> Unit,
    productPreferences: ProductPreferences?,
    onFavoriteRequest: ((Boolean) -> Unit)?
) {
    Surface(color = Color.White) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(8.dp)
        ) {
            IconButton(
                onClick = onBackRequest, modifier = Modifier
                    .padding(2.dp)
                    .background(Grey, shape = CircleShape)
            ) {
                Icon(
                    imageVector = Icons.Default.ArrowBackIosNew,
                    contentDescription = "Back"
                )
            }

            Spacer(modifier = Modifier.weight(1f))

            if (onFavoriteRequest != null) {
                productPreferences?.let {
                    IconButton(onClick = {
                        onFavoriteRequest(!it.isFavourite)
                    }, modifier = Modifier.padding(8.dp)) {
                        Icon(
                            imageVector = if (it.isFavourite) Icons.Default.Favorite else Icons.Default.FavoriteBorder,
                            contentDescription = "Favorite",
                        )
                    }
                } ?: CircularProgressIndicator()
            }
        }
    }
}