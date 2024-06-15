package pt.isel.markettracker.ui.screens.profile

import android.util.Log
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue

@Composable
fun ProfileScreen(
    profileScreenViewModel: ProfileScreenViewModel
) {
    val user by profileScreenViewModel.userPhase.collectAsState()

    val avatar = profileScreenViewModel.avatarPath

    LaunchedEffect(Unit) {
        profileScreenViewModel.fetchUser()
        Log.v("User", "já foi executado o fetchUser")
    }

    ProfileScreenView(
        userState = user,
        avatar = avatar,
        onLogoutRequested = profileScreenViewModel::logout,
        onUpdateUserRequested = profileScreenViewModel::updateUser
    )
}