package pt.isel.markettracker.ui.screens.profile

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Settings
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.http.models.user.UserOutputModel
import pt.isel.markettracker.ui.components.common.IOResourceLoader
import pt.isel.markettracker.ui.screens.profile.components.AvatarIcon
import pt.isel.markettracker.ui.screens.profile.components.SettingsButton
import pt.isel.markettracker.ui.screens.profile.components.TimeDisplay
import pt.isel.markettracker.ui.theme.mainFont

const val ProfileScreenTestTag = "SignUpScreenTag"

@Composable
fun ProfileScreen(
    user: IOState<UserOutputModel>,
    onFetchUserRequested: () -> Unit,
    onLogoutRequested: () -> Unit,
    onEditRequested: () -> Unit,
    onChangeAvatarRequested: () -> Unit,
    onDeleteAccountRequested: () -> Unit
) {

    LaunchedEffect(Unit) {
        onFetchUserRequested()
    }

    IOResourceLoader(
        resource = user,
        errorContent = {
            Text("Falha ao carregar o profile!")
        },
    ) { userDetails ->

        Box(
            modifier = Modifier
                .fillMaxSize()
                .testTag(ProfileScreenTestTag),
            contentAlignment = Alignment.Center
        ) {
            SettingsButton(
                icon = Icons.Default.Settings,
                onEditRequested = onEditRequested,
                onDeleteRequested = onDeleteAccountRequested,
                modifier = Modifier.align(alignment = Alignment.TopEnd)
            )

            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(24.dp)
            ) {

                Text(
                    text = "Bem vindo/a ${userDetails.name} 👋",
                    fontFamily = mainFont,
                    fontSize = 30.sp
                )

                AvatarIcon(
                    avatarIcon = userDetails.avatar,
                    onIconClick = onChangeAvatarRequested,
                )

                Text(
                    text = userDetails.username,
                    fontFamily = mainFont
                )

                Text(
                    text = userDetails.email
                )

                TimeDisplay(
                    userDetails.createdAt
                )

                Button(
                    onClick = onLogoutRequested
                ) {
                    Text("Logout ✋", fontFamily = mainFont)
                }
            }
        }
    }
}
