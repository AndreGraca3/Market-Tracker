package pt.isel.markettracker.ui.screens.list.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp

@Composable
fun ListStatusIcons(
    isOwner: Boolean,
    numberOfParticipants: Int,
) {
    Column(
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(20.dp)
    ) {
        OwnershipStatusIcon(isOwner = isOwner)
        ParticipantBadge(numberOfParticipants = numberOfParticipants)
    }
}