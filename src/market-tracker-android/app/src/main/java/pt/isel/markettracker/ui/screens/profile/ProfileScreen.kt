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
        Log.v("User", "Vai ser executado o fetchUser")
    }

    ProfileScreenView(
        userState = user,
        onLogoutRequested = profileScreenViewModel::logout,
        onUpdateUserRequested = profileScreenViewModel::updateUser
    )
}