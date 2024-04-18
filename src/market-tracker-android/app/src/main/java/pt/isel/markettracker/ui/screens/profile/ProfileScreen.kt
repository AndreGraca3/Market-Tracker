package pt.isel.markettracker.ui.screens.profile

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.domain.user.User
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.ui.theme.mainFont

const val ProfileScreenTestTag = "SignUpScreenTag"

@Composable
fun ProfileScreen(
    user: User
) {
    MarkettrackerTheme {
        Box(
            modifier = Modifier
                .fillMaxSize()
                .testTag(ProfileScreenTestTag),
            contentAlignment = Alignment.Center
        ) {
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(24.dp)
            ) {

                Image(
                    painter = painterResource(R.drawable.user_icon),
                    contentDescription = "user_icon"
                )

                Text(
                    text = user.username
                )

                Text(
                    text = user.name
                )

                Text(
                    text = user.email
                )

                Button(onClick = { /*TODO*/ }) {
                    Text("Logout", fontFamily = mainFont)
                }
            }
        }
    }
}