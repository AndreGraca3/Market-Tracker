package pt.isel.markettracker.ui.screens.listDetails.cards

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.list.listEntry.ListEntryOffer
import pt.isel.markettracker.dummy.dummyShoppingListEntries
import pt.isel.markettracker.ui.components.LoadableImage
import pt.isel.markettracker.ui.screens.listDetails.components.ProductQuantityCounter
import pt.isel.markettracker.ui.screens.products.card.ProductCardSpecs

@Composable
fun ProductListCard(
    productEntry: ListEntryOffer,
    isEditing: Boolean,
    isInCheckBoxMode: Boolean = false,
    onQuantityIncreaseRequest: () -> Unit,
    onQuantityDecreaseRequest: () -> Unit,
    isLoading: Boolean,
    loadingContent: @Composable () -> Unit,
) {
    val shape = RoundedCornerShape(8.dp)
    val product = productEntry.productOffer.product

    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier
            .size(width = 350.dp, 130.dp)
    ) {
        Card(
            modifier = Modifier
                .fillMaxSize()
                .padding(2.dp)
                .border(2.dp, Color.Black.copy(.6F), shape),
            colors = CardDefaults.cardColors(Color.White)
        ) {
            if (isLoading) {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    loadingContent()
                }
            } else {
                Row(
                    horizontalArrangement = Arrangement.spacedBy(10.dp),
                    verticalAlignment = Alignment.CenterVertically,
                    modifier = Modifier
                        .fillMaxSize()
                        .padding(14.dp, 8.dp)
                ) {
                    Box(
                        contentAlignment = Alignment.Center,
                        modifier = Modifier
                            .fillMaxHeight()
                            .fillMaxWidth(.2F)
                    ) {
                        LoadableImage(
                            url = product.imageUrl,
                            contentDescription = product.name,
                            modifier = Modifier.fillMaxSize()
                        )
                    }

                    Box(
                        contentAlignment = Alignment.CenterStart,
                        modifier = Modifier
                            .fillMaxHeight()
                            .fillMaxWidth(.6F)
                    ) {
                        ProductCardSpecs(product = product)
                    }

                    Box(
                        modifier = Modifier
                            .fillMaxHeight(0.3F)
                            .fillMaxWidth(),
                        contentAlignment = Alignment.Center
                    ) {
                        ProductQuantityCounter(
                            quantity = productEntry.quantity,
                            enabled = !isEditing,
                            onQuantityIncreaseRequest = onQuantityIncreaseRequest,
                            onQuantityDecreaseRequest = onQuantityDecreaseRequest
                        )
                    }

                    if (isInCheckBoxMode) {
                        TODO("Not yet implemented")
                    }
                }
            }
        }
    }
}

@Preview
@Composable
fun ProductListCardPreview() {
    ProductListCard(
        productEntry = dummyShoppingListEntries.entries.first(),
        isEditing = false,
        onQuantityIncreaseRequest = {},
        onQuantityDecreaseRequest = {},
        isLoading = false,
        loadingContent = {}
    )
}