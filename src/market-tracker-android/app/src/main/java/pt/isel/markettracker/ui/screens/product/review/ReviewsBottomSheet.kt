package pt.isel.markettracker.ui.screens.product.review

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Close
import androidx.compose.material3.BottomSheetDefaults
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.ModalBottomSheet
import androidx.compose.material3.Text
import androidx.compose.material3.rememberModalBottomSheetState
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ReviewsBottomSheet(
    showBottomSheet: Boolean,
    onDismissRequest: () -> Unit
) {
    val sheetState = rememberModalBottomSheetState()

    if (showBottomSheet) {
        ModalBottomSheet(
            onDismissRequest = onDismissRequest,
            sheetState = sheetState,
            dragHandle = {
                Row(
                    modifier = Modifier
                        .padding(18.dp, 14.dp)
                        .fillMaxWidth(),
                    verticalAlignment = Alignment.CenterVertically,
                ) {
                    Box(modifier = Modifier.weight(1f)) {
                        Text(text = "Reviews", fontWeight = FontWeight.Bold)
                    }
                    BottomSheetDefaults.DragHandle()
                    Box(
                        modifier = Modifier.weight(1f),
                        contentAlignment = Alignment.CenterEnd
                    ) {
                        IconButton(onClick = onDismissRequest) {
                            Icon(imageVector = Icons.Default.Close, contentDescription = "Close")
                        }
                    }
                }
            }
        ) {
            Column(
                modifier = Modifier
                    .padding(10.dp, 14.dp)
                    .verticalScroll(rememberScrollState())
                    .border(1.dp, Color.Black),
                verticalArrangement = Arrangement.spacedBy(10.dp)
            ) {
                (1..100).forEach {
                    ReviewTile()
                }
            }
        }
    }
}