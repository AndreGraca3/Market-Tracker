package pt.isel.markettracker.ui.screens.product.components

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.AddAlert
import androidx.compose.material.icons.filled.ArrowBackIosNew
import androidx.compose.material.icons.filled.Favorite
import androidx.compose.material.icons.filled.HeartBroken
import androidx.compose.material.icons.filled.RemoveCircle
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Surface
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Loaded
import pt.isel.markettracker.domain.extractValue
import pt.isel.markettracker.domain.product.ProductPreferences
import pt.isel.markettracker.ui.theme.Grey

@Composable
fun ProductTopBar(
    onBackRequest: () -> Unit,
    preferencesState: IOState<ProductPreferences>,
    onAlertRequest: () -> Unit,
    onFavoriteRequest: () -> Unit
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

            when (preferencesState) {
                is Loaded -> {
                    val preferences = preferencesState.extractValue()

                    IconButton(onClick = onAlertRequest, modifier = Modifier.padding(8.dp)) {
                        Icon(
                            imageVector = if (preferences.priceAlert != null) Icons.Default.RemoveCircle
                            else Icons.Default.AddAlert,
                            contentDescription = "Alert",
                        )
                    }

                    IconButton(onClick = onFavoriteRequest, modifier = Modifier.padding(8.dp)) {
                        Icon(
                            imageVector = if (preferences.isFavorite) Icons.Default.HeartBroken else Icons.Default.Favorite,
                            contentDescription = "Favorite",
                        )
                    }
                }

                else -> {
                    CircularProgressIndicator()
                }
            }
        }
    }
}