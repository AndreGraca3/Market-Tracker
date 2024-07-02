package pt.isel.markettracker.ui.screens.list.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp

@Composable
fun ListStatusIcons(
    isOwner: Boolean,
    numberOfParticipants: Int,
    modifier: Modifier,
) {
    Box(
        modifier = modifier
            .padding(
                horizontal = 20.dp
            ),
        contentAlignment = Alignment.Center
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(20.dp)
        ) {
            OwnershipStatusIcon(isOwner = isOwner)
            ParticipantBadge(numberOfParticipants = numberOfParticipants)
        }
    }
}