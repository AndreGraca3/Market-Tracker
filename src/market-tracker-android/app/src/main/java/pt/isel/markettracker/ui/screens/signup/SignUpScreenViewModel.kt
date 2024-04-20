package pt.isel.markettracker.ui.screens.signup

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.fail
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.domain.loaded
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.http.models.StringIdOutputModel
import pt.isel.markettracker.http.models.user.UserCreationInputModel
import pt.isel.markettracker.http.service.operations.user.IUserService
import javax.inject.Inject

@HiltViewModel
class SignUpScreenViewModel @Inject constructor(
    private val userService: IUserService
) : ViewModel() {

    private val signUpPhaseFlow: MutableStateFlow<IOState<StringIdOutputModel>> =
        MutableStateFlow(idle())

    val signUpPhase: Flow<IOState<StringIdOutputModel>>
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
                false -> fail(Exception("Invalid Credentials"))
            }
        }
    }
}