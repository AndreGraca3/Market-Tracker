package pt.isel.markettracker.ui.screens.product

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.Comment
import androidx.compose.material.icons.filled.AddAlert
import androidx.compose.material.icons.filled.ArrowBackIosNew
import androidx.compose.material.icons.filled.Favorite
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.ui.components.LoadableImage
import pt.isel.markettracker.ui.screens.product.review.ReviewsBottomSheet
import pt.isel.markettracker.ui.theme.Grey
import pt.isel.markettracker.ui.theme.MarketTrackerTypography

@Composable
fun ProductDetailsScreen(
    onBackRequest: () -> Unit,
    viewModel: ProductDetailsScreenViewModel = hiltViewModel()
) {
    var isReviewsSectionOpen by remember { mutableStateOf(false) }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .verticalScroll(rememberScrollState())
    ) {
        ProductTopBar(onBackRequest = onBackRequest)
        Box(
            modifier = Modifier
                .fillMaxWidth()
                .height(350.dp)
                .clip(RoundedCornerShape(bottomStart = 46.dp, bottomEnd = 46.dp))
                .background(Color.White),
            contentAlignment = Alignment.Center
        ) {
            LoadableImage(
                model = dummyProducts.first().imageUrl,
                contentDescription = "Product Image",
                modifier = Modifier
                    .padding(16.dp)
            )
        }

        Column(
            modifier = Modifier.padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            Text(text = dummyProducts.first().name, style = MarketTrackerTypography.titleLarge)

            IconButton(onClick = { isReviewsSectionOpen = true }) {
                Icon(
                    imageVector = Icons.AutoMirrored.Filled.Comment, contentDescription = "Reviews"
                )
            }


            Text(text = "Prices", style = MarketTrackerTypography.titleMedium)

            (1..30).forEach {
                Text(text = "Price $it", style = MarketTrackerTypography.bodyMedium)
            }
        }

        ReviewsBottomSheet(
            isReviewsSectionOpen,
            onDismissRequest = { isReviewsSectionOpen = false }
        )
    }
}