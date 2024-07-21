package pt.isel.markettracker.ui.screens.priceHistory

import android.os.Build
import android.os.Bundle
import android.os.Parcelable
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.parcelize.Parcelize
import pt.isel.markettracker.ui.theme.MarkettrackerTheme

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

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            MarkettrackerTheme {
                PriceHistoryScreen(
                    productId = productExtra.id,
                    storeId = productExtra.storeId,
                    onBackRequested = { finish() }
                )
            }
        }
    }
}

@Parcelize
data class ProductExtra(val id: String, val storeId: Int) : Parcelable