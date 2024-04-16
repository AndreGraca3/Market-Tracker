package pt.isel.markettracker.http.service.operations.auth

import android.app.Activity
import android.util.Log
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.ActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.runtime.Composable
import com.google.android.gms.auth.api.signin.GoogleSignIn
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.auth.api.signin.GoogleSignInClient
import com.google.android.gms.auth.api.signin.GoogleSignInOptions
import com.google.android.gms.tasks.Task


object AuthService {


    private const val TAG = "GoogleAuth"
    private const val SERVER_ID =
        "317635904868-mgmlu2g27gt43tb00c7i5kfevprerrsn.apps.googleusercontent.com"

    fun getGoogleLoginAuth(ctx: Activity): GoogleSignInClient {
        val gso = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
            .requestIdToken(SERVER_ID)
            //.requestId()
            //.requestProfile()
            .requestEmail()
            .build()
        return GoogleSignIn.getClient(ctx, gso)
    }


    @Composable
    fun StartGoogleAuthentication() {
        rememberLauncherForActivityResult(ActivityResultContracts.StartActivityForResult()) { result: ActivityResult ->
            if (result.resultCode == Activity.RESULT_OK) {
                Log.v(TAG, "Google sign in result: $result")
                val intent = result.data
                if (intent != null) {
                    val task: Task<GoogleSignInAccount> =
                        GoogleSignIn.getSignedInAccountFromIntent(intent)
                    handleTask(task)
                } else {
                    Log.e(TAG, "Google sign in failed: intent is null")
                }
            } else {
                Log.e(TAG, "Google sign in failed: result code is not OK: ${result.resultCode}")
            }
        }
    }

    private fun handleTask(task: Task<GoogleSignInAccount>) {
        Log.v(TAG, "Google sign in task: $task")
        task.result?.let {
            Log.d(TAG, "Result success: ${it.idToken}")
        } ?: run {
            Log.e(TAG, "Result faul: ${task.exception}")
        }

        task.addOnSuccessListener {
            Log.d(TAG, "Google sign in successful: ${it.displayName}")
        }

        task.addOnFailureListener {
            Log.e(TAG, "Google sign in failed: ${it.message}")
        }
    }
}