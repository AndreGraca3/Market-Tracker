package pt.isel.markettracker.ui.components.common

import android.widget.Toast
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import com.talhafaki.composablesweettoast.util.SweetToastUtil.SweetError
import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.Loading
import pt.isel.markettracker.domain.exceptionOrNull
import pt.isel.markettracker.domain.getOrNull

@Composable
fun <T> IOResourceLoader(
    resource: IOState<T>,
    loadingMessage: String = "A carregar...",
    errorContent: @Composable () -> Unit = {},
    loadingContent: (@Composable () -> Unit)? = null,
    content: @Composable (T) -> Unit,
) {
    Box(
        modifier = Modifier.fillMaxSize(),
        contentAlignment = Alignment.Center
    ) {
        when (resource) {
            is Loading -> {
                if (loadingContent == null) {
                    LoadingIcon(loadingMessage)
                } else loadingContent()
            }

            else -> {
                val ex = resource.exceptionOrNull()
                if (ex != null) {
                    SweetError(
                        ex.message ?: "Unknown error",
                        Toast.LENGTH_LONG,
                        contentAlignment = Alignment.Center
                    )
                    errorContent()
                } else {
                    val data = resource.getOrNull()
                    if (data != null) {
                        content(data)
                    }
                }
            }
        }
    }
}