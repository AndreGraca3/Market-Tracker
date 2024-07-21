package pt.isel.markettracker.ui.screens.priceHistory

import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import com.talhafaki.composablesweettoast.util.SweetToastUtil
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.market.price.PriceAlert
import pt.isel.markettracker.ui.screens.product.alert.PriceAlertDialog
import pt.isel.markettracker.ui.screens.product.alert.PriceAlertState

@Composable
fun PriceHistoryScreen(
    productId: String,
    storeId: Int,
    currentPrice: Int,
    alert: PriceAlert?,
    checkOrRequestNotificationPermission: (() -> Unit) -> Unit,
    onBackRequested: () -> Unit,
    priceHistoryScreenViewModel: PriceHistoryScreenViewModel = hiltViewModel(),
) {
    val historyState by priceHistoryScreenViewModel.priceHistory.collectAsState()
    val priceAlertState by priceHistoryScreenViewModel.priceAlertStateFlow.collectAsState()
    var showAlertDialog by rememberSaveable { mutableStateOf(false) }

    LaunchedEffect(Unit) {
        priceHistoryScreenViewModel.fetchPriceHistory(productId, storeId)
    }

    PriceHistoryScreenView(
        state = historyState,
        hasAlert = alert != null,
        onAlertClick = {
            if (alert != null) priceHistoryScreenViewModel.deleteAlert(alert.id)
            else {
                checkOrRequestNotificationPermission {
                    showAlertDialog = true
                }
            }
        },
        onBackRequested = onBackRequested
    )

    when (priceAlertState) {
        is PriceAlertState.Created -> {
            SweetToastUtil.SweetSuccess(
                message = stringResource(id = R.string.alert_set),
                contentAlignment = Alignment.BottomCenter,
                padding = PaddingValues(top = 28.dp)
            )
        }

        is PriceAlertState.Deleted -> {
            SweetToastUtil.SweetSuccess(
                message = stringResource(id = R.string.alert_removed),
                contentAlignment = Alignment.BottomCenter,
                padding = PaddingValues(top = 28.dp)
            )
        }

        is PriceAlertState.Error -> {
            SweetToastUtil.SweetError(
                message = stringResource(id = R.string.alert_error),
                contentAlignment = Alignment.BottomCenter,
                padding = PaddingValues(top = 28.dp)
            )
        }

        else -> {}
    }

    PriceAlertDialog(
        showAlertDialog = showAlertDialog,
        price = currentPrice,
        onAlertSet = { priceThreshold ->
            priceHistoryScreenViewModel.createAlert(productId, storeId, priceThreshold)
            showAlertDialog = false
        },
        onDismissRequest = { showAlertDialog = false }
    )
}