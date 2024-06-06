package pt.isel.markettracker.navigation

import androidx.compose.animation.core.Animatable
import androidx.compose.animation.core.tween
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Icon
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.scale
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.unit.dp
import com.exyte.animatednavbar.AnimatedNavigationBar
import com.exyte.animatednavbar.animation.balltrajectory.Straight
import com.exyte.animatednavbar.animation.indendshape.shapeCornerRadius
import com.exyte.animatednavbar.utils.noRippleClickable
import pt.isel.markettracker.ui.theme.Primary
import pt.isel.markettracker.ui.theme.Primary700

@Composable
fun NavBar(navBarItems: List<Destination>, selectedIndex: Int, onItemClick: (String) -> Unit) {
    AnimatedNavigationBar(
        modifier = Modifier.height(60.dp),
        selectedIndex = selectedIndex,
        ballAnimation = Straight(
            tween(500)
        ),
        cornerRadius = shapeCornerRadius(
            topLeft = 28.dp,
            topRight = 28.dp,
            bottomLeft = 0.dp,
            bottomRight = 0.dp
        ),
        barColor = Primary,
        ballColor = Primary700
    ) {
        navBarItems.forEach { item ->
            val itemIndex = navBarItems.indexOf(item)
            Box(
                modifier = Modifier
                    .fillMaxSize()
                    .noRippleClickable {
                        if (selectedIndex == itemIndex) return@noRippleClickable
                        onItemClick(item.route)
                    },
                contentAlignment = Alignment.Center
            ) {
                val scale = remember { Animatable(1f) }

                LaunchedEffect(key1 = selectedIndex) {
                    if (selectedIndex == itemIndex) {
                        scale.animateTo(
                            targetValue = 1.3f,
                            animationSpec = tween(200)
                        )
                        scale.animateTo(
                            targetValue = 1.2f,
                            animationSpec = tween(200)
                        )
                    } else {
                        scale.animateTo(
                            targetValue = 1f,
                            animationSpec = tween(200)
                        )
                    }
                }

                Icon(
                    modifier = Modifier
                        .size(26.dp)
                        .scale(scale.value),
                    imageVector = item.icon,
                    contentDescription = item.route,
                    tint = if (selectedIndex == itemIndex) Color.Black
                    else Color.White.copy(0.8F)
                )
            }
        }
    }
}