package pt.isel.markettracker.ui.screens.products.grid

import androidx.compose.animation.AnimatedVisibility
import androidx.compose.animation.core.tween
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.animation.scaleIn
import androidx.compose.animation.scaleOut
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.KeyboardArrowUp
import androidx.compose.material.icons.filled.KeyboardDoubleArrowUp
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.IconButtonDefaults
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.ui.theme.Primary500
import pt.isel.markettracker.ui.theme.Primary600
import pt.isel.markettracker.ui.theme.Primary700

@Composable
fun ScrollToTopButton(
    visible: Boolean,
    onClick: () -> Unit
) {
    Box(
        Modifier
            .fillMaxSize()
            .padding(bottom = 30.dp, end = 20.dp),
        contentAlignment = Alignment.BottomEnd
    ) {
        AnimatedVisibility(
            visible = visible,
            enter = fadeIn(tween(200)) + scaleIn(tween(200)),
            exit = fadeOut(tween(200)) + scaleOut(tween(200))
        ) {
            IconButton(
                onClick = onClick,
                modifier = Modifier
                    .clip(shape = CircleShape)
                    .size(50.dp)
                    .border(2.dp, Primary700, CircleShape),
                colors = IconButtonDefaults.iconButtonColors(
                    containerColor = Primary500,
                    contentColor = Color.White
                )
            ) {
                Icon(Icons.Filled.KeyboardDoubleArrowUp, "scroll to top", modifier = Modifier.fillMaxSize(0.8F))
            }
        }
    }
}