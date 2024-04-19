package pt.isel.markettracker.ui.screens.login

import android.content.Context
import android.content.Intent
import android.util.Log
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.core.content.ContextCompat.getString
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.markettracker.R
import com.google.android.gms.auth.api.signin.GoogleSignIn
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.auth.api.signin.GoogleSignInOptions
import com.google.android.gms.tasks.Task
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.models.token.TokenCreationInputModel
import pt.isel.markettracker.http.service.operations.token.ITokenService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import javax.inject.Inject

private const val TAG = "GoogleAuth"

@HiltViewModel
class LoginScreenViewModel @Inject constructor(
    private val tokenService: ITokenService,
    //private val preferences: PreferencesRepository
) : ViewModel() {

    companion object {
        fun getGoogleLoginIntent(ctx: Context): Intent {
            val options = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestIdToken(getString(ctx, R.string.GOOGLE_CLIENT_ID))
                //.requestId()
                //.requestProfile()
                .requestEmail()
                .build()
            val client = GoogleSignIn.getClient(ctx, options)
            client.revokeAccess() // this is here so it asks all the time for consent
            return client.signInIntent
        }
    }


    private val loginPhaseFlow: MutableStateFlow<LoginScreenState> =
        MutableStateFlow(LoginScreenState.Idle)

    val loginPhase: Flow<LoginScreenState>
        get() = loginPhaseFlow.asStateFlow()


    var email by mutableStateOf("")
    var password by mutableStateOf("")

    fun login() {
        if (loginPhaseFlow.value is LoginScreenState.Loading || email.isEmpty() || password.isEmpty()) return
        loginPhaseFlow.value = LoginScreenState.Loading

        viewModelScope.launch {
            val result = runCatchingAPIFailure {
                tokenService.createToken(
                    TokenCreationInputModel(
                        email, password
                    )
                )
            }
            if (result.isSuccess) {
                loginPhaseFlow.value = LoginScreenState.Success(result)
                return@launch
            }
            loginPhaseFlow.value = LoginScreenState.Fail(result.exceptionOrNull()!!)
        }
    }

    fun handleGoogleSignInTask(task: Task<GoogleSignInAccount>) {
        loginPhaseFlow.value = LoginScreenState.Loading
        task.addOnSuccessListener {
            Log.d(TAG, "Result success: ${it.idToken}")
            Log.d(TAG, "Google sign in successful: ${it.displayName}")
            loginPhaseFlow.value = LoginScreenState.Success(it)
        }
        task.addOnFailureListener {
            Log.e(TAG, "Google sign in failed: ${it.message}")
            Log.e(TAG, "Result fail: ${task.exception}")
            loginPhaseFlow.value = LoginScreenState.Fail(it)
        }
    }

    fun logout() {
        email = ""
        password = ""
        loginPhaseFlow.value = LoginScreenState.Idle
    }
}