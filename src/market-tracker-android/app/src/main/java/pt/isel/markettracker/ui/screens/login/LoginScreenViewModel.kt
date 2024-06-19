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
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import pt.isel.markettracker.http.models.token.GoogleTokenCreationInputModel
import pt.isel.markettracker.http.models.token.TokenCreationInputModel
import pt.isel.markettracker.http.service.operations.auth.IAuthService
import pt.isel.markettracker.http.service.result.runCatchingAPIFailure
import javax.inject.Inject

private const val TAG = "GoogleAuth"

@HiltViewModel
class LoginScreenViewModel @Inject constructor(
    private val authService: IAuthService
) : ViewModel() {

    companion object {
        fun getGoogleLoginIntent(ctx: Context): Intent {
            val options = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestIdToken(getString(ctx, R.string.GOOGLE_CLIENT_ID))
                .requestProfile()
                .requestEmail()
                .build()
            val client = GoogleSignIn.getClient(ctx, options)
            //client.revokeAccess() // this is here so it asks all the time for consent
            return client.signInIntent
        }
    }

    private val loginPhaseFlow: MutableStateFlow<LoginScreenState> =
        MutableStateFlow(LoginScreenState.Idle)

    val loginPhase
        get() = loginPhaseFlow.asStateFlow()

    var email by mutableStateOf("")
    var password by mutableStateOf("")

    fun login() {
        if (loginPhaseFlow.value is LoginScreenState.Loading || email.isEmpty() || password.isEmpty()) return
        loginPhaseFlow.value = LoginScreenState.Loading

        viewModelScope.launch {
            val res = runCatchingAPIFailure {
                authService.signIn(
                    TokenCreationInputModel(
                        email, password
                    )
                )
            }

            res.onSuccess {
                loginPhaseFlow.value = LoginScreenState.Loaded
            }

            res.onFailure {
                loginPhaseFlow.value = LoginScreenState.Fail(it)
            }
        }
    }

    fun handleGoogleSignInTask(task: Task<GoogleSignInAccount>) {
        loginPhaseFlow.value = LoginScreenState.Loading
        task.addOnSuccessListener { googleSignInAccount ->
            Log.d(TAG, "Result success: ${googleSignInAccount.idToken}")
            Log.d(TAG, "Google sign in successful: ${googleSignInAccount.displayName}")
            viewModelScope.launch {
                val res = runCatchingAPIFailure {
                    authService.googleSignIn(GoogleTokenCreationInputModel(googleSignInAccount.idToken!!))
                }

                res.onSuccess {
                    loginPhaseFlow.value = LoginScreenState.Loaded
                }

                res.onFailure {
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

    fun logout() {
        loginPhaseFlow.value = LoginScreenState.Idle
    }
}