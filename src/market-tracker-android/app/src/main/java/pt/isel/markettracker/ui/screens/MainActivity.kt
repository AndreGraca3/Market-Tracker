package pt.isel.markettracker.ui.screens

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import com.google.mlkit.vision.barcode.common.Barcode
import com.google.mlkit.vision.codescanner.GmsBarcodeScannerOptions
import com.google.mlkit.vision.codescanner.GmsBarcodeScanning
import dagger.hilt.android.AndroidEntryPoint
import pt.isel.markettracker.ui.screens.product.ProductDetailsActivity
import pt.isel.markettracker.ui.screens.product.ProductIdExtra
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.utils.navigateTo

@AndroidEntryPoint
class MainActivity : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        installSplashScreen().setKeepOnScreenCondition {
            // upload FCM token to server
            false
        }
        super.onCreate(savedInstanceState)

        setContent {
            MarkettrackerTheme {
                MainScreen(
                    onProductClick = {
                        navigateTo<ProductDetailsActivity>(
                            this,
                            ProductDetailsActivity.PRODUCT_ID_EXTRA,
                            ProductIdExtra(it)
                        )
                    },
                    onBarcodeScanRequest = {
                        val scanner =
                            GmsBarcodeScanning.getClient(this, barcodeScannerOptions)

                        scanner.startScan().addOnSuccessListener {
                            navigateTo<ProductDetailsActivity>(
                                this,
                                ProductDetailsActivity.PRODUCT_ID_EXTRA,
                                ProductIdExtra(it.rawValue ?: "")
                            )
                        }
                    }
                )
            }
        }
    }

    private val barcodeScannerOptions by lazy {
        GmsBarcodeScannerOptions.Builder()
            .setBarcodeFormats(Barcode.FORMAT_EAN_13)
            .setBarcodeFormats(Barcode.FORMAT_EAN_8)
            .build()
    }
}