package pt.isel.markettracker.ui.screens.priceHistory

import android.Manifest
import android.content.pm.PackageManager
import android.os.Build
import android.os.Bundle
import android.os.Parcelable
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.compose.setContent
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.core.content.ContextCompat
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.parcelize.Parcelize
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.repository.auth.extractAlerts
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import javax.inject.Inject

@AndroidEntryPoint
class PriceHistoryActivity : ComponentActivity() {

    companion object {
        const val PRODUCT_EXTRA = "productExtra"
    }

    private val productExtra by lazy {
        val extra = if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU)
            intent.getParcelableExtra(
                PRODUCT_EXTRA,
                ProductExtra::class.java
            )
        else
            intent.getParcelableExtra(PRODUCT_EXTRA)

        checkNotNull(extra) { "No product id extra found in intent" }
        extra
    }

    @Inject
    lateinit var authRepository: IAuthRepository

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            val permissionLauncher = rememberLauncherForActivityResult(
                ActivityResultContracts.RequestPermission()
            ) { isGranted ->
                if (!isGranted) {
                    Toast.makeText(
                        this,
                        "Permission to access notifications is required",
                        Toast.LENGTH_SHORT
                    ).show()
                }
            }

            val authState by authRepository.authState.collectAsState()
            val alert = authState.extractAlerts()
                .find { it.product.id == productExtra.id && it.store.id == productExtra.storeId }

            MarkettrackerTheme {
                PriceHistoryScreen(
                    productId = productExtra.id,
                    storeId = productExtra.storeId,
                    currentPrice = productExtra.currentPrice,
                    alert = alert,
                    checkOrRequestNotificationPermission = { callback ->
                        if (checkNotificationPermission()) {
                            callback()
                        } else {
                            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
                                permissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
                            }
                        }
                    },
                    onBackRequested = { finish() }
                )
            }
        }
    }

    private fun checkNotificationPermission(): Boolean {
        return if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
            ContextCompat.checkSelfPermission(
                this,
                Manifest.permission.POST_NOTIFICATIONS
            ) == PackageManager.PERMISSION_GRANTED
        } else {
            true
        }
    }
}

@Parcelize
data class ProductExtra(val id: String, val storeId: Int, val currentPrice: Int) : Parcelable