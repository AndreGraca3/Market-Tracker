package pt.isel.markettracker.ui.screens.product.stores

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.ModalBottomSheet
import androidx.compose.material3.rememberModalBottomSheetState
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun StoresBottomSheet(
    showStores: Boolean,
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
                    .padding(12.dp, 0.dp)
                    .verticalScroll(rememberScrollState()),
                verticalArrangement = Arrangement.spacedBy(21.dp),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                (1..10).forEach {
                    StoreTile(
                        storeName = "Continente",
                        storeAddress = "Rua do Continente",
                        storeCity = "Lisboa",
                        storePrice = 199,
                        onStoreSelected = {
                            // TODO: select store
                            onDismissRequest()
                        }
                    )
                    HorizontalDivider(modifier = Modifier.fillMaxWidth(0.9F))
                }
            }
        }
    }
}