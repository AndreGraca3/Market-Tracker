package pt.isel.markettracker.ui.screens.login

sealed class LoginScreenState {
    data object Loading : LoginScreenState()

    data object Idle : LoginScreenState()

    data class Fail(val error: Throwable) : LoginScreenState()

    data object Success : LoginScreenState()
}