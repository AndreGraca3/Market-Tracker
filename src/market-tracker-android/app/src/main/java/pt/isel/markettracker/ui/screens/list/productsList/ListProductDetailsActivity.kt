package pt.isel.markettracker.ui.screens.list.productsList

import android.os.Bundle
import android.os.Parcelable
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.lifecycle.lifecycleScope
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.launch
import kotlinx.parcelize.Parcelize
import pt.isel.markettracker.ui.theme.MarkettrackerTheme

@AndroidEntryPoint
class ListProductDetailsActivity : ComponentActivity() {
    companion object {
        const val LIST_PRODUCT_ID_EXTRA = "listProductIdExtra"
    }

    private val listProductId by lazy {
        val extra = if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.TIRAMISU)
            intent.getParcelableExtra(
                LIST_PRODUCT_ID_EXTRA,
                ListIdExtra::class.java
            )
        else
            intent.getParcelableExtra(LIST_PRODUCT_ID_EXTRA)

        checkNotNull(extra) { "No list product id extra found in intent" }
        extra.id
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        lifecycleScope.launch {
            //vm.listProduct.collect { state ->
            //    if (state is Idle) vm.fetchListProductById(productId)
            //    if(state is Loaded) {
            //        vm.fetchListProductStats(productId)
            //    }
            //}
        }

        setContent {
            MarkettrackerTheme {
                ListProductDetailsScreen(onBackRequest = { finish() })
            }
        }
    }
}

@Parcelize
data class ListIdExtra(val id: String) : Parcelable