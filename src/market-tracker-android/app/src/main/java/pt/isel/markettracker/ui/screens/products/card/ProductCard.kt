package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.product.ProductInfo
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.dummy.dummyStores
import pt.isel.markettracker.ui.components.LoadableImage
import pt.isel.markettracker.ui.components.button.AddToListButton
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.ui.theme.Primary900
import pt.isel.markettracker.utils.advanceShadow

@Composable
fun ProductCard(product: ProductInfo, onProductClick: (String) -> Unit) {
    val shape = RoundedCornerShape(8.dp)
    Card(
        modifier = Modifier
            .fillMaxSize()
            .clip(shape)
            .clickable { onProductClick(product.id) }
            .padding(2.dp)
            .border(2.dp, Color.Black.copy(.6F), shape)
            .advanceShadow(Primary900, blurRadius = 24.dp),
        colors = CardDefaults.cardColors(Color.White)
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(10.dp),
            modifier = Modifier
                .fillMaxSize()
                .padding(14.dp, 8.dp)
        ) {
            Box(
                contentAlignment = Alignment.Center,
                modifier = Modifier
                    .fillMaxWidth()
                    .fillMaxHeight(.4F)
            ) {
                LoadableImage(
                    model = product.imageUrl,
                    contentDescription = product.name,
                    modifier = Modifier.fillMaxSize()
                )
            }
            ProductCardSpecs(product = product)
            CompanyPriceCardHeader(
                69,
                dummyStores.first().company.logoUrl
            )

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