package pt.isel.markettracker.navigation

import androidx.compose.animation.core.animateFloatAsState
import androidx.compose.animation.core.tween
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.size
import androidx.compose.material3.Icon
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableFloatStateOf
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.rotate
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
fun NavBar(navBarItems: List<Destination>, onItemClick: (String) -> Unit) {
    var selectedIndex by remember { mutableIntStateOf(0) }

    AnimatedNavigationBar(
        modifier = Modifier.height(64.dp),
        selectedIndex = selectedIndex,
        ballAnimation = Straight(
            tween(500)
        ),
        cornerRadius = shapeCornerRadius(
            topLeft = 34.dp,
            topRight = 34.dp,
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
                        selectedIndex = itemIndex
                        onItemClick(item.route)
                    },
                contentAlignment = Alignment.Center
            ) {
                val scale by animateFloatAsState(
                    if (selectedIndex == itemIndex) 1.3F else 1F,
                    label = "scale"
                )

                val rotation by animateFloatAsState(
                    if (selectedIndex == itemIndex) 360F else 0F,
                    label = "rotation",
                    animationSpec = tween(if (selectedIndex == itemIndex) 300 else 0)
                )

                Icon(
                    modifier = Modifier
                        .size(26.dp)
                        .rotate(rotation)
                        .scale(scale),
                    imageVector = item.icon,
                    contentDescription = item.route,
                    tint = if (selectedIndex == itemIndex) Color.Black
                    else Color.White.copy(0.8F)
                )
            }
        }
    }
}