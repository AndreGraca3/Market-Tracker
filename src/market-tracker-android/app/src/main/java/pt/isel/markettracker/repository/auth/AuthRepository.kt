package pt.isel.markettracker.repository.auth

import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import javax.inject.Inject

class AuthRepository @Inject constructor() : IAuthRepository {

    private val _authState = MutableStateFlow<AuthEvent>(AuthEvent.Idle)
    override val authState
        get() = _authState.asStateFlow()

    override fun isUserLoggedIn(): Boolean {
        return _authState.value !is AuthEvent.Idle
    }

    override fun login() {
        _authState.value = AuthEvent.Login
    }

    override fun logout() {
        _authState.value = AuthEvent.Logout
    }
}

sealed class AuthEvent {
    data object Idle : AuthEvent()
    data object Login : AuthEvent()
    data object Logout : AuthEvent()
}