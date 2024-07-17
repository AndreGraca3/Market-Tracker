package pt.isel.markettracker.ui.screens.users

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import dagger.hilt.android.AndroidEntryPoint
import pt.isel.markettracker.ui.screens.listDetails.ListIdExtra
import pt.isel.markettracker.ui.theme.MarkettrackerTheme

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

        checkNotNull(extra) { "No list id extra found in intent" }
        extra.id
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            MarkettrackerTheme {
                UsersScreen(
                    listId = listId,
                    onBackRequested = { finish() }
                )
            }
        }
    }
}