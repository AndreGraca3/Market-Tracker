package pt.isel.markettracker.ui.screens.login

import android.util.Log
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.tasks.Task
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.service.operations.auth.IAuthService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import javax.inject.Inject

private const val TAG = "GoogleAuth"

@HiltViewModel
class LoginScreenViewModel @Inject constructor(
    private val authService: IAuthService
) : ViewModel() {
    private val loginPhaseFlow: MutableStateFlow<LoginScreenState> =
        MutableStateFlow(LoginScreenState.Idle)
    val loginPhase
        get() = loginPhaseFlow.asStateFlow()

    var email by mutableStateOf("test@g")
    var password by mutableStateOf("123")

    fun login() {
        if (loginPhaseFlow.value is LoginScreenState.Loading || email.isEmpty() || password.isEmpty()) return
        loginPhaseFlow.value = LoginScreenState.Loading

        viewModelScope.launch {
            runCatchingAPIFailure {
                authService.signIn(email, password)
            }.onSuccess {
                loginPhaseFlow.value = LoginScreenState.Success
            }.onFailure {
                loginPhaseFlow.value = LoginScreenState.Fail(it)
            }
        }
    }

    fun handleGoogleSignInTask(task: Task<GoogleSignInAccount>) {
        Log.d(TAG, "Handling google sign in task")
        loginPhaseFlow.value = LoginScreenState.Loading
        task.addOnSuccessListener { googleSignInAccount ->
            val idToken = googleSignInAccount.idToken
            Log.d(TAG, "Result success: $idToken")

            if (idToken == null) {
                loginPhaseFlow.value =
                    LoginScreenState.Fail(Exception("Google idToken is null"))
                return@addOnSuccessListener
            }

            viewModelScope.launch {
                runCatchingAPIFailure {
                    authService.googleSignIn(idToken)
                }.onSuccess {
                    loginPhaseFlow.value = LoginScreenState.Success
                }.onFailure {
                    loginPhaseFlow.value = LoginScreenState.Fail(it)
                }
            }
        }

        task.addOnFailureListener {
            Log.e(TAG, "Google sign in failed: ${it.message}")
            Log.e(TAG, "Result fail: ${task.exception}")
            loginPhaseFlow.value = LoginScreenState.Fail(it)
        }
    }
}