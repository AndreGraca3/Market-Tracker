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
import androidx.compose.material3.Button
import androidx.compose.material3.LocalTextStyle
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.ui.components.common.IOResourceLoader
import pt.isel.markettracker.ui.components.text.MarketTrackerTextField
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
    name: String,
    username: String,
    email: String,
    onNameChangeRequested: (String) -> Unit,
    onUsernameChangeRequested: (String) -> Unit,
    onEmailChangeRequested: (String) -> Unit,
    onLogoutRequested: () -> Unit,
    onUpdateAvatarPath: (Uri) -> Unit,
    onUpdateUserRequested: () -> Unit,
) {

    var isInEditMode by rememberSaveable { mutableStateOf(false) }

    val launcher =
        rememberLauncherForActivityResult(
            contract = ActivityResultContracts.GetContent(),
            onResult = { contentPath ->
                if (contentPath != null) {
                    onUpdateAvatarPath(contentPath)
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
                        onEditRequested = { isInEditMode = true },
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
                contentAlignment = Alignment.TopCenter
            ) {
                Column(
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(24.dp),
                    modifier = Modifier.padding(20.dp)
                ) {
                    AsyncAvatarIcon(
                        avatarIcon = avatar,
                        isEditing = isInEditMode,
                        onIconClick = {
                            launcher.launch("image/*")
                        }
                    )

                    DisplayUserInfo(
                        name = name,
                        onNameChangeRequested = onNameChangeRequested,
                        username = username,
                        onUsernameChangeRequested = onUsernameChangeRequested,
                        email = email,
                        onEmailChangeRequested = onEmailChangeRequested,
                        isInEditMode = isInEditMode
                    )
                }

                Box(
                    modifier = Modifier
                        .fillMaxSize()
                        .padding(vertical = 10.dp),
                    contentAlignment = Alignment.BottomCenter
                ) {
                    if (isInEditMode) {
                        Row(
                            verticalAlignment = Alignment.CenterVertically,
                            horizontalArrangement = Arrangement.Center
                        ) {
                            Box(
                                modifier = Modifier
                                    .weight(0.5F),
                                contentAlignment = Alignment.Center
                            ) {
                                Button(
                                    onClick = {
                                        onUpdateUserRequested()
                                        isInEditMode = false
                                    }
                                ) {
                                    Text(
                                        text = "Guardar ✔️"
                                    )
                                }
                            }

                            Box(
                                modifier = Modifier
                                    .weight(0.5F),
                                contentAlignment = Alignment.Center
                            ) {
                                Button(
                                    onClick = {
                                        isInEditMode = false
                                    }
                                ) {
                                    Text(
                                        text = "Cancelar ❌"
                                    )
                                }
                            }
                        }
                    } else {
                        TimeDisplay(
                            userDetails.createdAt
                        )
                    }
                }
            }
        }
    }
}

@Composable
fun DisplayUserInfo(
    name: String,
    onNameChangeRequested: (String) -> Unit,
    username: String,
    onUsernameChangeRequested: (String) -> Unit,
    email: String,
    onEmailChangeRequested: (String) -> Unit,
    isInEditMode: Boolean
) {
    if (isInEditMode) {
        MarketTrackerTextField(
            value = name,
            onValueChange = onNameChangeRequested,
            textStyle = LocalTextStyle.current.copy(textAlign = TextAlign.Center)
        )

        MarketTrackerTextField(
            value = username,
            onValueChange = onUsernameChangeRequested,
            textStyle = LocalTextStyle.current.copy(textAlign = TextAlign.Center)
        )

        MarketTrackerTextField(
            value = email,
            onValueChange = onEmailChangeRequested,
            textStyle = LocalTextStyle.current.copy(textAlign = TextAlign.Center)
        )
    } else {
        Text(
            text = name,
            fontFamily = mainFont,
            fontSize = 20.sp
        )

        Text(
            text = username,
            fontFamily = mainFont,
            fontSize = 20.sp
        )

        Text(
            text = email,
            fontFamily = mainFont,
            fontSize = 20.sp
        )
    }
}