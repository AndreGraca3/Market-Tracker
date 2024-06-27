package pt.isel.markettracker.ui.screens.product

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
import androidx.activity.viewModels
import androidx.core.content.ContextCompat
import androidx.lifecycle.lifecycleScope
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.launch
import kotlinx.parcelize.Parcelize
import pt.isel.markettracker.repository.auth.IAuthRepository
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import javax.inject.Inject

@AndroidEntryPoint
class ProductDetailsActivity : ComponentActivity() {

    companion object {
        const val PRODUCT_ID_EXTRA = "productIdExtra"
    }

    private val productId by lazy {
        val extra = if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.TIRAMISU)
            intent.getParcelableExtra(
                PRODUCT_ID_EXTRA,
                ProductIdExtra::class.java
            )
        else
            intent.getParcelableExtra(PRODUCT_ID_EXTRA)

        checkNotNull(extra) { "No product id extra found in intent" }
        extra.id
    }

    private val vm by viewModels<ProductDetailsScreenViewModel>()

    @Inject
    lateinit var authRepository: IAuthRepository

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        lifecycleScope.launch {
            vm.stateFlow.collect { state ->
                if (state is ProductDetailsScreenState.Idle) vm.fetchProductById(productId)
                if (state is ProductDetailsScreenState.LoadedProduct) {
                    vm.fetchProductDetails(productId)
                }
            }
        }

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

            MarkettrackerTheme {
                ProductDetailsScreen(
                    onBackRequest = { finish() },
                    checkOrRequestNotificationPermission = { callback ->
                        if (checkNotificationPermission()) {
                            callback()
                        } else {
                            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
                                permissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
                            }
                        }
                    },
                    vm,
                    authRepository
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
data class ProductIdExtra(val id: String) : Parcelable
