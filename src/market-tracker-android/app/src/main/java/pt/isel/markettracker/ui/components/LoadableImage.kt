package pt.isel.markettracker.ui.components

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.animation.scaleIn
import androidx.compose.animation.scaleOut
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Error
import androidx.compose.material3.Icon
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.painter.Painter
import androidx.compose.ui.layout.ContentScale
import coil.compose.SubcomposeAsyncImage
import pt.isel.markettracker.ui.components.common.LoadingIcon

@Composable
fun LoadableImage(
    url: String,
    contentDescription: String,
    modifier: Modifier = Modifier
) {
    var isLoading by rememberSaveable(url) { mutableStateOf(true) }
    var painter by remember { mutableStateOf<Painter?>(null) }

    SubcomposeAsyncImage(
        model = url,
        contentDescription = contentDescription,
        loading = { LoadingIcon() },
        success = {
            painter = it.painter
            isLoading = false
        },
        error = {
            Icon(
                Icons.Filled.Error,
                "Error loading image",
                modifier = Modifier.fillMaxSize(0.3F)
            )
        }
    )

    AnimatedVisibility(
        visible = !isLoading,
        enter = fadeIn() + scaleIn(),
        exit = fadeOut() + scaleOut()
    ) {

        if (painter == null) {
            return@AnimatedVisibility
        }

        Image(
            painter = painter!!,
            contentDescription = contentDescription,
            contentScale = ContentScale.Fit,
            modifier = modifier
        )
    }
}