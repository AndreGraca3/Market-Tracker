package pt.isel.markettracker.ui.screens.profile

import android.content.ContentResolver
import android.net.Uri
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.assisted.Assisted
import dagger.assisted.AssistedInject
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.domain.Fail
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Idle
import pt.isel.markettracker.domain.idle
import pt.isel.markettracker.domain.loadSuccess
import pt.isel.markettracker.domain.loading
import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.http.service.operations.token.ITokenService
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.utils.convertImageToBase64

@HiltViewModel(assistedFactory = ProfileScreenViewModelFactory::class)
class ProfileScreenViewModel @AssistedInject constructor(
    @Assisted private val contentResolver: ContentResolver,
    private val userService: IUserService,
    private val authRepository: IAuthRepository,
    private val tokenService: ITokenService
) : ViewModel() {

    private val clientFetchingFlow: MutableStateFlow<IOState<Client>> =
        MutableStateFlow(idle())

    val userPhase
        get() = clientFetchingFlow.asStateFlow()

    var avatarPath by mutableStateOf<Uri?>(null)

    fun fetchUser() {
        if (clientFetchingFlow.value !is Idle) return
        clientFetchingFlow.value = loading()
        viewModelScope.launch {
            val result = runCatchingAPIFailure {
                userService.getUser("1")
            }
            if (result.isSuccess) {
                avatarPath?.let { uri -> convertImageToBase64(contentResolver, uri) }
                clientFetchingFlow.value = loadSuccess(result.getOrNull()!!)
                return@launch
            }
            clientFetchingFlow.value = Fail(result.exceptionOrNull()!!)
        }
    }

    fun resetToIdle() {
        //remove token from preferences
        authRepository.logout()
        viewModelScope.launch {
            val user = userService.getUser("1")
            tokenService.deleteToken(user.id)
        }
        clientFetchingFlow.value = idle()
    }

    fun deleteAccount() {
        viewModelScope.launch {
            tokenService.deleteToken("1")
            userService.deleteUser("1")
        }
        clientFetchingFlow.value = idle()
    }

    fun updateUser() {
        viewModelScope.launch {
            userService.updateUserAvatar("1", avatarPath.toString())
        }
    }
}