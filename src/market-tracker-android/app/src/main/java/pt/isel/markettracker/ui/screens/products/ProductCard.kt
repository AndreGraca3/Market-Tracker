package pt.isel.markettracker.ui.screens.products

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Euro
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.unit.dp
import coil.compose.AsyncImage
import coil.compose.SubcomposeAsyncImage
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.ui.components.common.LoadingIcon

@Composable
fun ProductCard(product: ProductInfo) {
    Card(
        colors = CardDefaults.cardColors(Color.White),
        elevation = CardDefaults.cardElevation(32.dp),
        modifier = Modifier
            .padding(8.dp)
            .size(120.dp, 210.dp)
            .clip(RoundedCornerShape(8.dp))
            .shadow(32.dp, shape = RoundedCornerShape(8.dp))
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.Center,
            modifier = Modifier
                .fillMaxSize()
                .padding(8.dp)
                //.background(Color.Cyan)
        ) {
            SubcomposeAsyncImage(
                model = product.imageUrl,
                contentDescription = "${product.name} image",
                contentScale = ContentScale.FillBounds,
                loading = {
                  LoadingIcon()
                },
                modifier = Modifier.size(120.dp)
            )
            Spacer(modifier = Modifier.height(8.dp))
            Text(
                text = product.name,
                color = Color.Black
            )
            Row {
                Icon(
                    imageVector = Icons.Default.Euro,
                    contentDescription = "Store icon"
                )
                Text(
                    text = "${product.lowestPriceStore.price}"
                )
            }
        }
    }
}