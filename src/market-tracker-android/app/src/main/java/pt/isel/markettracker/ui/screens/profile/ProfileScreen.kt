package pt.isel.markettracker.ui.screens.profile

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun ProfileScreen(
    profileScreenViewModel: ProfileScreenViewModel
) {
    val user by profileScreenViewModel.userPhase.collectAsState()

    LaunchedEffect(Unit){
        profileScreenViewModel.fetchUser()
    }

    ProfileScreenView(
        userState = user,
        onLogoutRequested = profileScreenViewModel::logout,
        onUpdateUserRequested = profileScreenViewModel::updateUser
    )
}