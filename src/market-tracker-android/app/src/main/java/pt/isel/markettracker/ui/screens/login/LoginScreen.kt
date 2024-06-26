package pt.isel.markettracker.ui.screens.login

import android.content.Intent
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue

@Composable
fun LoginScreen(
    onSignUpRequested: () -> Unit,
    getGoogleLoginIntent: () -> Intent,
    onSuggestionRequested: () -> Unit = {},
    loginScreenViewModel: LoginScreenViewModel,
) {
    val loginState by loginScreenViewModel.loginPhase.collectAsState()

    LoginScreenView(
        state = loginState,
        email = loginScreenViewModel.email,
        password = loginScreenViewModel.password,
        onEmailChangeRequested = { loginScreenViewModel.email = it },
        onPasswordChangeRequested = { loginScreenViewModel.password = it },
        onSignUpRequested = onSignUpRequested,
        onLoginRequested = loginScreenViewModel::login,
        googleSignInHandler = loginScreenViewModel::handleGoogleSignInTask,
        getGoogleLoginIntent = getGoogleLoginIntent,
        onSuggestionRequested = onSuggestionRequested
    )
}