package pt.isel.markettracker.ui.screens.product

import android.os.Bundle
import android.os.Parcelable
import android.util.Log
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.lifecycle.lifecycleScope
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.launch
import kotlinx.parcelize.Parcelize
import pt.isel.markettracker.ui.theme.MarkettrackerTheme

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
            MarkettrackerTheme {
                ProductDetailsScreen(onBackRequest = { finish() }, vm)
            }
        }
    }
}

@Parcelize
data class ProductIdExtra(val id: String) : Parcelable
