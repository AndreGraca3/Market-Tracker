package pt.isel.markettracker.ui.screens.list.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R

@Composable
fun ListIconButtons(
    onCreateListRequested: () -> Unit,
    onCancelRequested: () -> Unit,
) {
    Column(
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(20.dp)
    ) {
        IconButton(
            onClick = onCreateListRequested
        ) {
            Icon(
                painterResource(id = R.drawable.check),
                contentDescription = null
            )
        }

        IconButton(
            onClick = onCancelRequested
        ) {
            Icon(
                painter = painterResource(id = R.drawable.cancel),
                contentDescription = null
            )
        }
    }
}