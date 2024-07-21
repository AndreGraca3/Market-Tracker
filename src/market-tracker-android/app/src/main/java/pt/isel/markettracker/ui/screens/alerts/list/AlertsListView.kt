package pt.isel.markettracker.ui.screens.alerts.list

import android.util.Log
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.alerts.AlertsScreenState
import pt.isel.markettracker.ui.screens.alerts.card.AlertProductCard
import pt.isel.markettracker.ui.screens.alerts.extractAlerts

@Composable
fun AlertsListView(
    state: AlertsScreenState,
    onDeleteProductFromAlertsRequested: (String) -> Unit,
) {
    when (state) {
        is AlertsScreenState.Loaded -> {
            val alerts = state.extractAlerts()
            if (alerts.isEmpty()) {
                Column(
                    horizontalAlignment = Alignment.CenterHorizontally,
                ) {
                    Image(
                        painter = painterResource(id = R.drawable.products_not_found),
                        contentDescription = "No products found"
                    )
                    Text(
                        text = stringResource(id = R.string.alerts_not_found)
                    )
                }
            } else {
                Box(
                    modifier = Modifier
                        .fillMaxWidth(),
                    contentAlignment = Alignment.TopCenter
                ) {
                    LazyColumn(
                        verticalArrangement = Arrangement.spacedBy(10.dp),
                        contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
                    ) {
                        Log.v("Alerts", "Alerts are $alerts")
                        items(alerts.size) { index ->
                            AlertProductCard(
                                priceAlert = alerts[index],
                                onRemoveFromAlertsRequested = {
                                    onDeleteProductFromAlertsRequested(alerts[index].id)
                                }
                            )
                        }
                    }
                }
            }
        }

        is AlertsScreenState.Loading -> {
            LoadingIcon(text = "Carregando os Alertas...")
        }


        else -> {
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
            ) {
                Image(
                    painter = painterResource(id = R.drawable.server_error),
                    contentDescription = "No products found"
                )
                Text(
                    text = stringResource(id = R.string.loading_error)
                )
            }
        }
    }
}