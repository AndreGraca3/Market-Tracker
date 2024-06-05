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
import pt.isel.markettracker.domain.Loading
import pt.isel.markettracker.domain.loadSuccess
import pt.isel.markettracker.domain.model.account.Client
import pt.isel.markettracker.http.models.user.UserUpdateInputModel
import pt.isel.markettracker.http.service.operations.auth.IAuthService
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.utils.convertImageToBase64

@HiltViewModel(assistedFactory = ProfileScreenViewModelFactory::class)
class ProfileScreenViewModel @AssistedInject constructor(
    @Assisted private val contentResolver: ContentResolver,
    private val userService: IUserService,
    private val authRepository: IAuthRepository,
    private val authService: IAuthService
) : ViewModel() {

    private val clientFetchingFlow: MutableStateFlow<IOState<Client>> =
        MutableStateFlow(Idle)

    val userPhase
        get() = clientFetchingFlow.asStateFlow()

    var avatarPath by mutableStateOf<Uri?>(null)

    fun fetchUser() {
        if (clientFetchingFlow.value !is Idle) return
        clientFetchingFlow.value = Loading()
        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                userService.getAuthenticatedUser()
            }

            res.onSuccess {
                avatarPath?.let { uri -> convertImageToBase64(contentResolver, uri) }
                clientFetchingFlow.value = loadSuccess(it)
            }

            res.onFailure {
                clientFetchingFlow.value = Fail(it)
            }
        }
    }

    fun logout() {
        viewModelScope.launch {
            authService.signOut()
        }
        clientFetchingFlow.value = Idle
    }

    fun deleteAccount(id: String) {
        viewModelScope.launch {
            //authRepository.setUserPreferences(token = null)
            authService.signOut()
            userService.deleteUser(id)
        }
        clientFetchingFlow.value = Idle
    }

    fun updateUser(uri: Uri) {
        viewModelScope.launch {
            userService.updateUser(
                "1",
                UserUpdateInputModel()
            )
        }
    }
}