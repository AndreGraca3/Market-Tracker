package pt.isel.markettracker.ui.screens.priceHistory

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun PriceHistoryScreen(
    productId: String,
    storeId: Int,
    onBackRequested: () -> Unit,
    priceHistoryScreenViewModel: PriceHistoryScreenViewModel = hiltViewModel(),
) {
    val historyState by priceHistoryScreenViewModel.priceHistory.collectAsState()

    LaunchedEffect(Unit) {
        priceHistoryScreenViewModel.fetchPriceHistory(productId, storeId)
    }

    PriceHistoryScreenView(
        state = historyState,
        onBackRequested = onBackRequested
    )
}