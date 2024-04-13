package pt.isel.markettracker.ui.screens.product

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.AddAlert
import androidx.compose.material.icons.filled.ArrowBackIosNew
import androidx.compose.material.icons.filled.Favorite
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.ui.components.LoadableImage
import pt.isel.markettracker.ui.theme.Grey
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun ProductDetailsScreen(
    onBackRequested: () -> Unit,
    viewModel: ProductDetailsScreenViewModel = hiltViewModel()
) {
    Scaffold(
        topBar = {
            Surface(
                color = Color.White
            ) {
                Row {
                    IconButton(
                        onClick = onBackRequested, modifier = Modifier
                            .background(Grey, shape = CircleShape)
                            .padding(8.dp)
                    ) {
                        Icon(
                            imageVector = Icons.Default.ArrowBackIosNew,
                            contentDescription = "Back"
                        )
                    }
                    Spacer(modifier = Modifier.weight(1f))

                    IconButton(onClick = { /*TODO*/ }, modifier = Modifier.padding(8.dp)) {
                        Icon(
                            imageVector = Icons.Default.AddAlert,
                            contentDescription = "Alert",
                        )
                    }

                    IconButton(onClick = { /*TODO*/ }, modifier = Modifier.padding(8.dp)) {
                        Icon(
                            imageVector = Icons.Default.Favorite,
                            contentDescription = "Favorite",
                        )
                    }
                }
            }
        }
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(it),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .height(350.dp)
                    .clip(RoundedCornerShape(bottomStart = 42.dp, bottomEnd = 42.dp))
                    .background(Color.White),
                contentAlignment = Alignment.Center
            ) {
                LoadableImage(
                    model = dummyProducts.first().imageUrl,
                    contentDescription = "Product Image",
                    modifier = Modifier
                        .padding(12.dp)
                )
            }

            Text(text = dummyProducts.first().name, style = MarketTrackerTypography.titleLarge)

            Text(text = "Prices", style = MarketTrackerTypography.titleMedium)
        }
    }
}