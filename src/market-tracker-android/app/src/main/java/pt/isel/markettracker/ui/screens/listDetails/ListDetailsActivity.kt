package pt.isel.markettracker.ui.screens.listDetails

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
import pt.isel.markettracker.ui.screens.users.UsersActivity
import pt.isel.markettracker.ui.theme.MarkettrackerTheme
import pt.isel.markettracker.utils.navigateTo

@AndroidEntryPoint
class ListDetailsActivity : ComponentActivity() {
    companion object {
        const val LIST_PRODUCT_ID_EXTRA = "listProductIdExtra"
    }

    private val listId by lazy {
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

    private val vm by viewModels<ListDetailsScreenViewModel>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        lifecycleScope.launch {
            vm.listDetails.collect { state ->
                if (state is ListDetailsScreenState.Idle) vm.fetchListDetails(listId)
            }
        }

        lifecycleScope.launch {
            vm.listDetails.collect { state ->
                Log.v(
                    "List",
                    "List state is $state and list is empty: ${state.extractShoppingListEntries().entries.isEmpty()}"
                )
            }
        }

        setContent {
            MarkettrackerTheme {
                ListDetailsScreen(
                    listId = listId,
                    onAddUsersToListRequested = {
                        navigateTo<UsersActivity>(
                            this,
                            UsersActivity.LIST_ID_EXTRA,
                            ListIdExtra(listId)
                        )
                    }
                )
            }
        }
    }
}

@Parcelize
data class ListIdExtra(val id: String) : Parcelable