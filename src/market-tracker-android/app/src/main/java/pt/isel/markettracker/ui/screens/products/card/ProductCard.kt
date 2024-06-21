package pt.isel.markettracker.ui.screens.products.card

import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.market.inventory.product.ProductOffer
import pt.isel.markettracker.dummy.dummyProducts
import pt.isel.markettracker.dummy.dummyStoreOffers
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.ui.theme.Primary900
import pt.isel.markettracker.utils.advanceShadow

@Composable
fun ProductCard(
    productOffer: ProductOffer,
    onProductClick: (String) -> Unit,
    onAddToListClick: () -> Unit,
    modifier: Modifier = Modifier
) {
    val shape = RoundedCornerShape(8.dp)

    Card(
        modifier = modifier
            .clip(shape)
            .clickable { onProductClick(productOffer.product.id) }
            .border(2.dp, Color.Black.copy(.6F), shape)
            .advanceShadow(Primary900, blurRadius = 24.dp),
        colors = CardDefaults.cardColors(Color.White)
    ) {
        ProductCardDetails(productOffer, onAddToListClick)
    }
}

@Preview
@Composable
fun ProductCardPreview() {
    MarkettrackerTheme {
        ProductCard(
            ProductOffer(
                dummyProducts.first(),
                dummyStoreOffers.first()
            ),
            onProductClick = {},
            onAddToListClick = {}
        )
    }
}