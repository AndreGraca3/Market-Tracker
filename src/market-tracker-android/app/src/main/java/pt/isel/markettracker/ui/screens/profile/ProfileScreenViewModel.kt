package pt.isel.markettracker.ui.screens.profile

import android.content.ContentResolver
import android.net.Uri
import android.util.Log
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
import pt.isel.markettracker.http.service.operations.alert.IAlertService
import pt.isel.markettracker.http.service.operations.auth.IAuthService
import pt.isel.markettracker.http.service.operations.list.IListService
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.repository.auth.IAuthRepository

@HiltViewModel(assistedFactory = ProfileScreenViewModelFactory::class)
class ProfileScreenViewModel @AssistedInject constructor(
    @Assisted private val contentResolver: ContentResolver,
    private val userService: IUserService,
    private val authService: IAuthService,
    private val listService: IListService,
    private val alertService: IAlertService,
    private val authRepository: IAuthRepository
) : ViewModel() {

    private val _clientFetchingFlow: MutableStateFlow<IOState<Client>> =
        MutableStateFlow(Idle)
    val clientFetchingFlow
        get() = _clientFetchingFlow.asStateFlow()

    var name by mutableStateOf("")
    var username by mutableStateOf("")
    var email by mutableStateOf("")

    var avatarPath by mutableStateOf<Uri?>(null)

    fun fetchUser() {
        if (_clientFetchingFlow.value !is Idle) return

        _clientFetchingFlow.value = Loading()

        viewModelScope.launch {
            runCatchingAPIFailure { userService.getAuthenticatedUser() }
                .onSuccess { client ->
                    // TODO: this should be in a state...
                    name = client.name
                    username = client.username
                    email = client.email
                    avatarPath = Uri.parse(client.avatar)
                    _clientFetchingFlow.value = loadSuccess(client)
                    Log.v("Avatar", "On fetch AvatarPath : $avatarPath")
                    Log.v("Avatar", "On fetch Avatar from db : ${client.avatar}")

                    this.launch {
                        authRepository.setToken(authRepository.getToken())

                        runCatchingAPIFailure { listService.getLists() }
                            .onSuccess {
                                authRepository.setLists(it)
                            }.onFailure {
                                _clientFetchingFlow.value = Fail(it)
                            }

                        runCatchingAPIFailure { alertService.getAlerts() }
                            .onSuccess { priceAlert ->
                                authRepository.setAlerts(priceAlert)
                            }.onFailure {
                                _clientFetchingFlow.value = Fail(it)
                            }
                    }
                }.onFailure {
                    // I think this is unnecessary, there is a handler to remove the token from repo
                    if (it.problem.status == 401) {
                        this.launch {
                            authRepository.setToken(null)
                        }
                    }
                    _clientFetchingFlow.value = Fail(it)
                }
        }
    }

    fun logout() {
        _clientFetchingFlow.value = Loading()
        viewModelScope.launch {
            authService.signOut()
            avatarPath = null
            _clientFetchingFlow.value = Idle
        }
    }

    fun updateUser() {
        if (_clientFetchingFlow.value is Loading || name.isBlank() || username.isBlank() || email.isBlank()) return
        _clientFetchingFlow.value = Loading()

        viewModelScope.launch {
            runCatchingAPIFailure {
                userService.updateUser(
                    name = name,
                    username = username,
                    avatar = avatarPath.toString()
                )
            }.onSuccess {
                Log.v("Avatar", "On Update AvatarPath : $avatarPath")
                Log.v("Avatar", "On Update Avatar from db : ${it.avatar}")
                avatarPath = Uri.parse(it.avatar)
                _clientFetchingFlow.value = loadSuccess(it)
            }.onFailure {
                _clientFetchingFlow.value = Fail(it)
            }
        }
    }

    fun deleteAccount() {
        viewModelScope.launch {
            authService.signOut()
            userService.deleteUser()
        }
        _clientFetchingFlow.value = Idle
    }
}