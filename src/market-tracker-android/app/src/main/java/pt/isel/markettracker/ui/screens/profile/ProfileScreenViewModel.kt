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
import kotlinx.coroutines.delay
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
import pt.isel.markettracker.http.service.operations.list.IListService
import pt.isel.markettracker.http.service.operations.user.IUserService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import pt.isel.markettracker.utils.convertImageToBase64

@HiltViewModel(assistedFactory = ProfileScreenViewModelFactory::class)
class ProfileScreenViewModel @AssistedInject constructor(
    @Assisted private val contentResolver: ContentResolver,
    private val userService: IUserService,
    private val authService: IAuthService,
    private val listService: IListService
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

            res.onSuccess { client ->
                viewModelScope.launch {
                    val listsRes = runCatchingAPIFailure {
                        listService.getLists(isOwner = true)
                    }

                    listsRes.onSuccess {
                        avatarPath?.let { uri -> convertImageToBase64(contentResolver, uri) }
                        clientFetchingFlow.value = loadSuccess(client)
                    }

                    listsRes.onFailure {
                        clientFetchingFlow.value = Fail(it)
                    }
                }

            }

            res.onFailure {
                clientFetchingFlow.value = Fail(it)
            }
        }
    }

    fun logout() {
        clientFetchingFlow.value = Loading()
        viewModelScope.launch {
            authService.signOut()
            clientFetchingFlow.value = Idle
        }
    }

    fun updateUser(contentPath: Uri?) {
        clientFetchingFlow.value = Loading()
        avatarPath = contentPath

        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                userService.updateUser(
                    "003b2463-f840-418b-85ea-0d26a9feef19",
                    UserUpdateInputModel(
                        avatar = avatarPath?.let { uri ->
                            convertImageToBase64(
                                contentResolver,
                                uri
                            )
                        }
                    )
                )
            }

            res.onSuccess {
                clientFetchingFlow.value = loadSuccess(it)
            }

            res.onFailure {
                clientFetchingFlow.value = Fail(it)
            }
        }
    }

    fun deleteAccount(id: String) {
        viewModelScope.launch {
            authService.signOut()
            userService.deleteUser(id)
        }
        clientFetchingFlow.value = Idle
    }
}