package pt.isel.markettracker.ui.screens.list.components

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R

@Composable
fun OwnershipStatusIcon(isOwner: Boolean) {
    if (isOwner) {
        Image(
            painter = painterResource(id = R.drawable.crown),
            contentDescription = "",
            modifier = Modifier
                .size(30.dp)
                .padding(end = 6.dp, top = 2.dp)
        )
    }
}
