package pt.isel.markettracker.ui.screens.products.list

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ModalBottomSheet
import androidx.compose.material3.Text
import androidx.compose.material3.rememberModalBottomSheetState
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.model.list.ShoppingList
import pt.isel.markettracker.ui.components.sheets.CustomDragHandle

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ListsBottomSheet(
    shoppingLists: List<ShoppingList>,
    onListSelectedClick: (String) -> Unit,
    onDismissRequest: () -> Unit
) {
    val sheetState = rememberModalBottomSheetState(skipPartiallyExpanded = true)
    val scrollState = rememberLazyListState()

    ModalBottomSheet(
        modifier = Modifier.fillMaxHeight(0.7F),
        onDismissRequest = onDismissRequest,
        sheetState = sheetState,
        dragHandle = {
            CustomDragHandle(
                title = stringResource(id = R.string.add_to_list),
                onDismissRequest = onDismissRequest
            )
        }
    ) {
        Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = Alignment.Center
        ) {
            if (shoppingLists.isEmpty()) {
                Text(
                    text = stringResource(id = R.string.no_lists),
                    color = Color.Gray,
                    fontWeight = FontWeight.Bold
                )
            } else {
                LazyColumn(
                    state = scrollState, contentPadding = PaddingValues(10.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp, Alignment.Top)
                ) {
                    items(shoppingLists.size) {
                        ListCard(
                            list = shoppingLists[it],
                            onListSelectedClick = onListSelectedClick
                        )
                    }
                }
            }
        }
    }
}