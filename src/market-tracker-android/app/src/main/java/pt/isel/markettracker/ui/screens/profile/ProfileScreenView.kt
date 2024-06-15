package pt.isel.markettracker.ui.screens.profile

import android.net.Uri
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.ui.components.common.IOResourceLoader
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo
import pt.isel.markettracker.ui.screens.profile.components.AsyncAvatarIcon
import pt.isel.markettracker.ui.screens.profile.components.SettingsButton
import pt.isel.markettracker.ui.screens.profile.components.TimeDisplay
import pt.isel.markettracker.ui.theme.mainFont

const val ProfileScreenTestTag = "SignUpScreenTag"

@Composable
fun ProfileScreenView(
    userState: IOState<Client>,
    avatar: Uri?,
    onLogoutRequested: () -> Unit,
    onUpdateUserRequested: (Uri) -> Unit,
) {


    val launcher =
        rememberLauncherForActivityResult(
            contract = ActivityResultContracts.GetContent(),
            onResult = { contentPath ->
                if (contentPath != null) {
                    onUpdateUserRequested(contentPath)
                }
            }
        )

    Scaffold(
        topBar = {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .background(Color.Red)
                    .padding(10.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                Box(
                    modifier = Modifier.fillMaxWidth()
                ) {
                    HeaderLogo(
                        modifier = Modifier
                            .align(alignment = Alignment.CenterStart)
                    )
                    Text(
                        "Profile 📝",
                        color = Color.White,
                        fontFamily = mainFont,
                        fontSize = 30.sp,
                        modifier = Modifier
                            .align(alignment = Alignment.Center)
                    )
                    SettingsButton(
                        onEditRequested = {

                        },
                        onLogoutRequested = onLogoutRequested,
                        modifier = Modifier.align(alignment = Alignment.CenterEnd)
                    )
                }
            }
        }
    ) { paddingValues ->
        IOResourceLoader(
            resource = userState,
            errorContent = {
                Text("Falha ao carregar o profile!")
            },
        ) { userDetails ->
            Box(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(paddingValues)
                    .testTag(ProfileScreenTestTag),
                contentAlignment = Alignment.Center
            ) {
                Column(
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(24.dp)
                ) {

                    Text(
                        text = "Bem vindo/a ${userDetails.username} 👋",
                        fontFamily = mainFont,
                        fontSize = 30.sp
                    )

                    AsyncAvatarIcon(
                        avatarIcon = avatar,
                        onIconClick = {
                            launcher.launch("image/*")
                        },
                    )

                    Text(
                        text = userDetails.username,
                        fontFamily = mainFont,
                        fontSize = 30.sp
                    )

                    TimeDisplay(
                        userDetails.createdAt
                    )
                }
            }
        }
    }
}