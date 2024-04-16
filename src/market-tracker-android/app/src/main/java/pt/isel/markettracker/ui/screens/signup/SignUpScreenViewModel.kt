package pt.isel.markettracker.ui.screens.signup

import android.util.Log
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
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.http.models.IdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure

class SignUpScreenViewModel(
    private val userService: IUserService
) : ViewModel() {
    companion object {
        fun factory(userService: IUserService) = viewModelFactory {
            initializer { SignUpScreenViewModel(userService) }
        }
    }

    private val signUpPhaseFlow: MutableStateFlow<IOState<IdOutputModel>> =
        MutableStateFlow(idle())

    val signUpPhase: Flow<IOState<IdOutputModel>>
        get() = signUpPhaseFlow.asStateFlow()

    var name by mutableStateOf("")
    var username by mutableStateOf("")
    var email by mutableStateOf("")
    var password by mutableStateOf("")

    fun createUser() {
        signUpPhaseFlow.value = loading()
        viewModelScope.launch {
            val result = runCatching {
                userService.createUser(
                    UserCreationInputModel(
                        name,
                        username,
                        email,
                        password,
                    )
                )
            }
            signUpPhaseFlow.value = when (result.isSuccess) {
                true -> loaded(result)
                false -> TODO("Not yet defined!")
            }
        }
    }
}