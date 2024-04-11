package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.SubcomposeAsyncImage
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.theme.MarkettrackerTheme

@Composable
fun ProductCard(product: ProductInfo, onProductClick: (String) -> Unit) {
    Card(
        modifier = Modifier
            .fillMaxSize()
            .clip(RoundedCornerShape(8.dp))
            .clickable { onProductClick(product.id) }
            .padding(2.dp)
            .shadow(4.dp, RoundedCornerShape(8.dp))
            .border(2.dp, Color.Black.copy(.6F), RoundedCornerShape(8.dp)),
        colors = CardDefaults.cardColors(Color.White)
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(8.dp),
            modifier = Modifier
                .fillMaxSize()
                .padding(8.dp)
        ) {
            SubcomposeAsyncImage(
                model = product.imageUrl,
                contentDescription = "${product.name} image",
                contentScale = ContentScale.Fit,
                loading = { LoadingIcon() },
                modifier = Modifier
                    .fillMaxWidth()
                    .fillMaxSize(.5F)
            )
            ProductSpecs(product = product)
            ProductPrice(product = product)
            AddToListButton(onClick = {})

        }
    }
}

@Preview
@Composable
fun ProductCardPreview() {
    MarkettrackerTheme {
        ProductCard(dummyProducts.first(), onProductClick = {})
    }
}