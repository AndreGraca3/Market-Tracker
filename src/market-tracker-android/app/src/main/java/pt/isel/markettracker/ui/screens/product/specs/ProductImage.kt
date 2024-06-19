package pt.isel.markettracker.ui.screens.product.specs

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.components.LoadableImage
import pt.isel.markettracker.utils.conditional
import pt.isel.markettracker.utils.shimmerEffect

@Composable
fun ProductImage(imageUrl: String?) {
    Box(
        modifier = Modifier
            .fillMaxWidth()
            .height(350.dp)
            .clip(RoundedCornerShape(bottomStart = 46.dp, bottomEnd = 46.dp))
            .background(Color.White)
            .conditional(imageUrl == null) { shimmerEffect() },
        contentAlignment = Alignment.Center
    ) {
        if (imageUrl != null) {
            LoadableImage(
                url = imageUrl,
                contentDescription = "Product Image",
                modifier = Modifier
                    .fillMaxSize()
                    .padding(18.dp)
            )
        }
    }
}