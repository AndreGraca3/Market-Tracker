package pt.isel.markettracker.ui.screens.profile

import android.content.ContentResolver
import android.net.Uri
import android.util.Log
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.google.firebase.messaging.FirebaseMessaging
import dagger.assisted.Assisted
import dagger.assisted.AssistedInject
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.async
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.service.operations.alert.IAlertService
import pt.isel.markettracker.http.service.operations.auth.IAuthService
import pt.isel.markettracker.http.service.operations.list.IListService
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.repository.auth.extractAlerts
import pt.isel.markettracker.repository.auth.extractLists
import pt.isel.markettracker.utils.convertImageToBase64

@HiltViewModel(assistedFactory = ProfileScreenViewModelFactory::class)
class ProfileScreenViewModel @AssistedInject constructor(
    @Assisted private val contentResolver: ContentResolver,
    private val userService: IUserService,
    private val authService: IAuthService,
    private val listService: IListService,
    private val alertService: IAlertService,
    private val authRepository: IAuthRepository,
) : ViewModel() {

    private val _clientFetchingFlow: MutableStateFlow<ProfileScreenState> =
        MutableStateFlow(ProfileScreenState.Idle)
    val clientFetchingFlow
        get() = _clientFetchingFlow.asStateFlow()

    var name by mutableStateOf("")
    var username by mutableStateOf("")

    var avatarPath by mutableStateOf<Uri?>(null)

    fun fetchUser() {
        if (_clientFetchingFlow.value !is ProfileScreenState.Idle) return
        Log.v("User", "Fetching user...")
        _clientFetchingFlow.value = ProfileScreenState.Loading

        viewModelScope.launch {
            runCatchingAPIFailure { userService.getAuthenticatedUser() }
                .onSuccess { client ->
                    name = client.name
                    username = client.username
                    Log.v("Avatar", "On fetch AvatarPath : ${avatarPath.toString().take(40)}")
                    Log.v(
                        "Avatar",
                        "On fetch Avatar from db : ${client.avatar.toString().take(40)}"
                    )

                    FirebaseMessaging.getInstance().token.addOnCompleteListener {
                        if (it.isSuccessful) {
                            val token = it.result
                            registerDevice(token)
                        }
                    }

                    this.launch {
                        // Fetch lists and alerts in parallel
                        val listsDeferred =
                            async { runCatchingAPIFailure { listService.getLists() } }
                        val alertsDeferred =
                            async { runCatchingAPIFailure { alertService.getAlerts() } }
                        val lists = listsDeferred.await()
                        val alerts = alertsDeferred.await()

                        if (lists.isFailure || alerts.isFailure) {
                            _clientFetchingFlow.value =
                                ProfileScreenState.Fail(Exception("Failed to fetch lists or alerts"))
                        } else {
                            val loadedLists = lists.getOrThrow()
                            val loadedAlerts = alerts.getOrThrow()
                            authRepository.setDetails(loadedLists, loadedAlerts)
                            _clientFetchingFlow.value =
                                ProfileScreenState.Success(
                                    client,
                                    loadedLists.size,
                                    0,
                                    loadedAlerts.size
                                )
                        }
                    }
                }.onFailure {
                    _clientFetchingFlow.value = ProfileScreenState.Fail(it)
                }
        }
    }

    fun logout() {
        _clientFetchingFlow.value = ProfileScreenState.Loading
        viewModelScope.launch {
            authService.signOut()
            avatarPath = null
            _clientFetchingFlow.value = ProfileScreenState.Idle
        }
    }

    fun updateUser() {
        if (_clientFetchingFlow.value is ProfileScreenState.Loading || name.isBlank() || username.isBlank()) return
        _clientFetchingFlow.value = ProfileScreenState.Loading

        viewModelScope.launch {
            runCatchingAPIFailure {
                userService.updateUser(
                    name = name,
                    username = username,
                    avatar = avatarPath.let { uri ->
                        uri?.let {
                            convertImageToBase64(
                                contentResolver,
                                it
                            )
                        }
                    }
                )
            }.onSuccess {
                Log.v("Avatar", "On Update AvatarPath : ${avatarPath.toString().take(40)}")
                Log.v("Avatar", "On Update Avatar from db : ${it.avatar.toString().take(40)}")
                _clientFetchingFlow.value = ProfileScreenState.Success(
                    it,
                    authRepository.authState.value.extractLists().size,
                    0,
                    authRepository.authState.value.extractAlerts().size
                )
            }.onFailure {
                _clientFetchingFlow.value = ProfileScreenState.Fail(it)
            }
        }
    }

    fun registerDevice(token: String) {
        viewModelScope.launch {
            val deviceId = authRepository.getOrGenerateDeviceId()
            runCatchingAPIFailure { userService.registerDevice(token, deviceId) }
        }
    }

    fun deleteAccount() {
        viewModelScope.launch {
            authService.signOut()
            userService.deleteUser()
        }
        _clientFetchingFlow.value = ProfileScreenState.Idle
    }

    fun updateLocalAvatar(uri: Uri?) {
        val currentState = _clientFetchingFlow.value
        if (currentState !is ProfileScreenState.Success) return
        avatarPath = uri
        if (uri != null) {
            _clientFetchingFlow.value = currentState.copy(
                client = currentState.client.copy(
                    avatar = convertImageToBase64(contentResolver, uri)
                )
            )
        }
    }

    fun resetToIdle() {
        _clientFetchingFlow.value = ProfileScreenState.Idle
    }
}