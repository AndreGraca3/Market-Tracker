package pt.isel.markettracker.ui.screens.products

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import coil.compose.AsyncImage
import pt.isel.markettracker.domain.product.ProductInfo

@Composable
fun ProductCard(product: ProductInfo) {
    Card(
        modifier = Modifier
            .padding(8.dp)
            .size(150.dp)
            .clip(RoundedCornerShape(8.dp)),
        elevation = CardDefaults.cardElevation(6.dp)
    ) {
        Column(
            modifier = Modifier.padding(8.dp)
        ) {
            AsyncImage(
                model = product.imageUrl,
                contentDescription = "${product.name} image",
            )
            Spacer(modifier = Modifier.height(8.dp))
            Text(
                text = product.name,
                color = Color.Black
            )
            Text(
                text = "$${product.lowestPriceStore.price}"
            )
        }
    }
}