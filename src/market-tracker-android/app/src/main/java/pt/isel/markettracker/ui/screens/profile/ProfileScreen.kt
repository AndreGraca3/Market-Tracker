package pt.isel.markettracker.ui.screens.profile

import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
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
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.hilt.navigation.compose.hiltViewModel
import pt.isel.markettracker.ui.components.common.IOResourceLoader
import pt.isel.markettracker.ui.screens.profile.components.AvatarIcon
import pt.isel.markettracker.ui.screens.profile.components.SettingsButton
import pt.isel.markettracker.ui.screens.profile.components.TimeDisplay
import pt.isel.markettracker.ui.theme.mainFont

const val ProfileScreenTestTag = "SignUpScreenTag"

@Composable
fun ProfileScreen(
    profileScreenViewModel: ProfileScreenViewModel
) {
    val user by profileScreenViewModel.userPhase.collectAsState()

    val launcher =
        rememberLauncherForActivityResult(
            contract = ActivityResultContracts.GetContent(),
            onResult = { contentPath ->
                if (contentPath != null) {
                    profileScreenViewModel.avatarPath = contentPath
                    profileScreenViewModel.updateUser()
                }
            }
        )

    LaunchedEffect(Unit) {
        profileScreenViewModel.fetchUser()
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
                onEditRequested = { },
                onDeleteRequested = {
                    profileScreenViewModel.resetToIdle()
                },
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
                    onIconClick = {
                        launcher.launch("image/*")
                    },
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
                    onClick = {
                        profileScreenViewModel.resetToIdle()
                    }
                ) {
                    Text("Logout ✋", fontFamily = mainFont)
                }
            }
        }
    }
}
