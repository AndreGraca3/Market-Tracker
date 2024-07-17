package pt.isel.markettracker.ui.screens.alerts

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun ListAlertsScreen(
    onBackRequested: () -> Unit,
    alertsScreenViewModel: AlertsScreenViewModel = hiltViewModel(),
) {
    val favoritesState by alertsScreenViewModel.alerts.collectAsState()

    LaunchedEffect(Unit) {
        alertsScreenViewModel.fetchAlerts()
    }

    ListAlertsScreenView(
        state = favoritesState,
        onRemoveFromAlertsRequested = {
            alertsScreenViewModel.removeFromAlerts(it)
        },
        onBackRequested = onBackRequested
    )
}