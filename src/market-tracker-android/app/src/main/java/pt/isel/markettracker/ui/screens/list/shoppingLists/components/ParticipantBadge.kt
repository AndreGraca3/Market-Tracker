package pt.isel.markettracker.ui.screens.list.shoppingLists.components

import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.offset
import androidx.compose.foundation.layout.size
import androidx.compose.material3.BadgedBox
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.sp
import pt.isel.markettracker.R

@Composable
fun ParticipantBadge(numberOfParticipants: Int) {
    BadgedBox(
        badge = {
            Text(
                text = numberOfParticipants.toString(),
                modifier = Modifier.offset(x = (0).dp, y = (-10).dp),
                style = androidx.compose.ui.text.TextStyle(fontSize = 10.sp)
            )
        }
    ) {
        Image(
            painter = painterResource(id = R.drawable.person),
            contentDescription = "",
            modifier = Modifier
                .size(32.dp)
        )
    }
}