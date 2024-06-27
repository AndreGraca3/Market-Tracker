package pt.isel.markettracker.ui.screens.profile

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue

@Composable
fun ProfileScreen(profileScreenViewModel: ProfileScreenViewModel) {
    val user by profileScreenViewModel.clientFetchingFlow.collectAsState()

    ProfileScreenView(
        userState = user,
        name = profileScreenViewModel.name,
        username = profileScreenViewModel.username,
        onNameChangeRequested = {
            profileScreenViewModel.name = it
        },
        onUsernameChangeRequested = {
            profileScreenViewModel.username = it
        },
        onLogoutRequested = profileScreenViewModel::logout,
        onUpdateAvatarPath = profileScreenViewModel::updateLocalAvatar,
        onUpdateUserRequested = profileScreenViewModel::updateUser,
        onDeleteAccountRequested = profileScreenViewModel::deleteAccount
    )
}