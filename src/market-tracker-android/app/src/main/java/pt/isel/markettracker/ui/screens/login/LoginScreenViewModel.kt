package pt.isel.markettracker.ui.screens.login

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Loading
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.domain.infrastruture.PreferencesRepository
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.http.models.token.TokenCreationInputModel
import pt.isel.markettracker.http.service.operations.token.ITokenService
import pt.isel.markettracker.http.service.result.APIResult
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure

class LoginScreenViewModel(
    private val tokenService: ITokenService,
    private val preferences: PreferencesRepository
) : ViewModel() {

    companion object {
        fun factory(tokenService: ITokenService, preferences: PreferencesRepository) =
            viewModelFactory {
                initializer { LoginScreenViewModel(tokenService, preferences) }
            }
    }

    private val loginPhaseFlow: MutableStateFlow<IOState<Unit>> =
        MutableStateFlow(idle())

    val loginPhase: Flow<IOState<Unit>>
        get() = loginPhaseFlow.asStateFlow()


    var email by mutableStateOf("")
    var password by mutableStateOf("")

    fun login() {
        if (loginPhaseFlow.value is Loading || email.isEmpty() || password.isEmpty()) return
        loginPhaseFlow.value = loading()

        viewModelScope.launch {
            val result = runCatchingAPIFailure {
                tokenService.createToken(
                    TokenCreationInputModel(
                        email, password
                    )
                )
            }
            loginPhaseFlow.value = loaded(Result.success(result.getOrNull()!!))
            preferences.setToken(result.getOrNull().toString())
        }
    }
}