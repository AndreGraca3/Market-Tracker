package pt.isel.markettracker.ui.screens.listDetails.cards

import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Checkbox
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.domain.model.list.listEntry.ListEntryOffer
import pt.isel.markettracker.dummy.dummyShoppingListEntries
import pt.isel.markettracker.ui.components.LoadableImage
import pt.isel.markettracker.ui.screens.listDetails.components.DisplayProductsStats
import pt.isel.markettracker.ui.screens.listDetails.components.ProductQuantityCounter
import pt.isel.markettracker.utils.centToEuro

@Composable
fun ProductListCard(
    productEntry: ListEntryOffer,
    isEditable: Boolean,
    isEditing: Boolean,
    isInCheckBoxMode: Boolean = false,
    onQuantityIncreaseRequest: () -> Unit,
    onQuantityDecreaseRequest: () -> Unit,
    isLoading: Boolean,
    loadingContent: @Composable () -> Unit,
) {
    val shape = RoundedCornerShape(8.dp)
    val productOffer = productEntry.productOffer
    val product = productOffer.product

    var showMaxValue by remember { mutableStateOf(false) }

    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier
            .size(width = 350.dp, 150.dp)
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

                    DisplayProductsStats(productOffer, Modifier.fillMaxWidth(.5F))

                    Box(
                        contentAlignment = Alignment.Center
                    ) {
                        Column {
                            if (isInCheckBoxMode) {
                                var isChecked by remember { mutableStateOf(false) }

                                Checkbox(
                                    checked = isChecked,
                                    onCheckedChange = { isChecked = !isChecked },
                                    modifier = Modifier.align(alignment = Alignment.End)
                                )
                            }

                            ProductQuantityCounter(
                                quantity = productEntry.quantity,
                                isEditable = isEditable,
                                enabled = !isEditing,
                                onQuantityIncreaseRequest = onQuantityIncreaseRequest,
                                onQuantityDecreaseRequest = onQuantityDecreaseRequest
                            )

                            Box(
                                modifier = Modifier.fillMaxWidth(),
                                contentAlignment = Alignment.Center
                            ) {
                                Text(
                                    text = if (showMaxValue)
                                        "${
                                            productEntry.productOffer.storeOffer.price.finalPrice.times(
                                                productEntry.quantity
                                            ).centToEuro()
                                        }€" else "${productEntry.productOffer.storeOffer.price.finalPrice.centToEuro()}€",
                                    color = Color.Black,
                                    textAlign = TextAlign.Center,
                                    modifier = Modifier.clickable { showMaxValue = !showMaxValue }
                                )
                            }
                        }
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
        isEditable = false,
        isEditing = false,
        onQuantityIncreaseRequest = {},
        onQuantityDecreaseRequest = {},
        isLoading = false,
        loadingContent = {}
    )
}