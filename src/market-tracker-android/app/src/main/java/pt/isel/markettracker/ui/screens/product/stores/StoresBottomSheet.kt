package pt.isel.markettracker.ui.screens.product.stores

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.ModalBottomSheet
import androidx.compose.material3.rememberModalBottomSheetState
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import pt.isel.markettracker.domain.model.price.StorePrice

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun StoresBottomSheet(
    showStores: Boolean,
    storesPrices: List<StorePrice>,
    onStoreSelect: (Int) -> Unit,
    onDismissRequest: () -> Unit
) {
    val sheetState = rememberModalBottomSheetState(skipPartiallyExpanded = true)

    if (showStores) {
        ModalBottomSheet(
            modifier = Modifier.fillMaxHeight(0.7F),
            onDismissRequest = onDismissRequest,
            sheetState = sheetState
        ) {
            Column(
                modifier = Modifier
                    .verticalScroll(rememberScrollState()),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                storesPrices.forEach {
                    StoreTile(
                        storeName = it.name,
                        storeAddress = it.address,
                        storeCity = it.city?.name,
                        storePrice = it.price,
                        onStoreSelected = {
                            onStoreSelect(it.id)
                            onDismissRequest()
                        }
                    )
                    HorizontalDivider(modifier = Modifier.fillMaxWidth(0.9F))
                }
            }
        }
    }
}