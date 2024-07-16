package pt.isel.markettracker.ui.screens.users

import androidx.activity.ComponentActivity
import dagger.hilt.android.AndroidEntryPoint
import pt.isel.markettracker.ui.screens.listDetails.ListIdExtra

@AndroidEntryPoint
class UsersActivity : ComponentActivity() {
    companion object {
        const val LIST_ID_EXTRA = "listIdExtra"
    }

    private val listId by lazy {
        val extra = if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.TIRAMISU)
            intent.getParcelableExtra(
                LIST_ID_EXTRA,
                ListIdExtra::class.java
            )
        else
            intent.getParcelableExtra(LIST_ID_EXTRA)

        checkNotNull(extra) { "No list product id extra found in intent" }
        extra.id
    }

}