package pt.isel.markettracker.ui.screens.profile

import android.util.Log
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue

@Composable
fun ProfileScreen(
    profileScreenViewModel: ProfileScreenViewModel
) {
    val user by profileScreenViewModel.userPhase.collectAsState()

    LaunchedEffect(Unit) {
        profileScreenViewModel.fetchUser()
    }

    ProfileScreenView(
        userState = user,
        avatar = profileScreenViewModel.avatarPath,
        name = profileScreenViewModel.name,
        username = profileScreenViewModel.username,
        email = profileScreenViewModel.email,
        onNameChangeRequested = {
            profileScreenViewModel.name = it
        },
        onUsernameChangeRequested = {
            profileScreenViewModel.username = it
        },
        onEmailChangeRequested = {
            profileScreenViewModel.email = it
        },
        onLogoutRequested = profileScreenViewModel::logout,
        onUpdateAvatarPath = {
            profileScreenViewModel.avatarPath = it
            Log.v("Avatar", "onUpdateAvatarPath AvatarPath: ${profileScreenViewModel.avatarPath}")
        },
        onUpdateUserRequested = profileScreenViewModel::updateUser
    )
}