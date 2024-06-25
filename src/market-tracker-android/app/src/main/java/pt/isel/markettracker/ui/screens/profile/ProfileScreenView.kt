package pt.isel.markettracker.ui.screens.profile

import android.net.Uri
import android.widget.Toast
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
import androidx.compose.foundation.layout.size
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
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.talhafaki.composablesweettoast.util.SweetToastUtil.SweetError
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.products.topbar.HeaderLogo
import pt.isel.markettracker.ui.screens.profile.components.AsyncAvatarIcon
import pt.isel.markettracker.ui.screens.profile.components.DisplayUserInfo
import pt.isel.markettracker.ui.screens.profile.components.SettingsButton
import pt.isel.markettracker.ui.theme.mainFont

const val ProfileScreenTestTag = "SignUpScreenTag"

@Composable
fun ProfileScreenView(
    userState: ProfileScreenState,
    avatar: Uri?,
    name: String,
    username: String,
    onNameChangeRequested: (String) -> Unit,
    onUsernameChangeRequested: (String) -> Unit,
    onLogoutRequested: () -> Unit,
    onUpdateAvatarPath: (Uri) -> Unit,
    onUpdateUserRequested: () -> Unit,
    onDeleteAccountRequested: () -> Unit
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
                            .size(48.dp)
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
                        onDeleteAccountRequested = onDeleteAccountRequested,
                        modifier = Modifier.align(alignment = Alignment.CenterEnd)
                    )
                }
            }
        }
    ) { paddingValues ->
        when (userState) {
            is ProfileScreenState.Loading -> {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    LoadingIcon()
                }
            }

            is ProfileScreenState.Fail -> {
                SweetError(
                    "Failed to fetch user.",
                    Toast.LENGTH_LONG,
                    contentAlignment = Alignment.Center
                )
            }

            is ProfileScreenState.Loaded -> {
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
                            username = username,
                            email = userState.client.email,
                            createdAt = userState.client.createdAt,
                            onNameChangeRequested = onNameChangeRequested,
                            onUsernameChangeRequested = onUsernameChangeRequested,
                            onSaveChangesRequested = {
                                onUpdateUserRequested()
                                isInEditMode = false
                            },
                            onCancelChangesRequested = {
                                isInEditMode = false
                            },
                            isInEditMode = isInEditMode,
                        )
                    }
                }
            }

            else -> {}
        }
    }
}

