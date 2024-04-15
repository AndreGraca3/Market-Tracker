package pt.isel.markettracker.ui.screens.product

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
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
import pt.isel.markettracker.ui.screens.product.prices.PricesSection
import pt.isel.markettracker.ui.screens.product.rating.ProductStatsRow
import pt.isel.markettracker.ui.screens.product.reviews.ReviewsBottomSheet
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
        ProductTopBar(
            onBackRequest = onBackRequest,
            hasAlert = false,
            onAlertRequest = {},
            isProductFavorite = true,
            onFavoriteRequest = {}
        )
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
                    .padding(18.dp)
            )
        }

        Column(
            modifier = Modifier.padding(20.dp, 16.dp),
            verticalArrangement = Arrangement.spacedBy(26.dp)
        ) {
            Text(text = dummyProducts.first().name, style = MarketTrackerTypography.titleLarge)

            ProductStatsRow(
                rating = 4.3,
                onCommunityReviewsRequest = { isReviewsSectionOpen = true }
            )

            PricesSection()
        }

        ReviewsBottomSheet(
            isReviewsSectionOpen,
            onDismissRequest = { isReviewsSectionOpen = false }
        )
    }
}