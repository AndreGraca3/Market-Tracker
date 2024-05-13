package pt.isel.markettracker.utils

import android.graphics.BlurMaskFilter
import androidx.compose.animation.core.EaseInOut
import androidx.compose.animation.core.LinearEasing
import androidx.compose.animation.core.RepeatMode
import androidx.compose.animation.core.animateFloat
import androidx.compose.animation.core.infiniteRepeatable
import androidx.compose.animation.core.rememberInfiniteTransition
import androidx.compose.animation.core.tween
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.offset
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.composed
import androidx.compose.ui.draw.drawBehind
import androidx.compose.ui.draw.scale
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.Paint
import androidx.compose.ui.graphics.drawscope.drawIntoCanvas
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.graphics.toArgb
import androidx.compose.ui.layout.onGloballyPositioned
import androidx.compose.ui.platform.LocalDensity
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.IntSize
import androidx.compose.ui.unit.dp

fun Modifier.advanceShadow(
    color: Color = Color.Black,
    borderRadius: Dp = 16.dp,
    blurRadius: Dp = 16.dp,
    offsetY: Dp = 0.dp,
    offsetX: Dp = 0.dp,
    spread: Float = 1f,
) = drawBehind {
    this.drawIntoCanvas {
        val paint = Paint()
        val frameworkPaint = paint.asFrameworkPaint()
        val spreadPixel = spread.dp.toPx()
        val leftPixel = (0f - spreadPixel) + offsetX.toPx()
        val topPixel = (0f - spreadPixel) + offsetY.toPx()
        val rightPixel = (this.size.width)
        val bottomPixel = (this.size.height + spreadPixel)

        if (blurRadius != 0.dp) {
            /*
                The feature maskFilter used below to apply the blur effect only works
                with hardware acceleration disabled.
             */
            frameworkPaint.maskFilter =
                (BlurMaskFilter(blurRadius.toPx(), BlurMaskFilter.Blur.NORMAL))
        }

        frameworkPaint.color = color.toArgb()
        it.drawRoundRect(
            left = leftPixel,
            top = topPixel,
            right = rightPixel,
            bottom = bottomPixel,
            radiusX = borderRadius.toPx(),
            radiusY = borderRadius.toPx(),
            paint
        )
    }
}

fun Modifier.shimmerEffect(): Modifier = composed {
    var size by remember {
        mutableStateOf(IntSize.Zero)
    }
    val transition = rememberInfiniteTransition(label = "shimmerTransition")
    val startOffsetX by transition.animateFloat(
        initialValue = -2 * size.width.toFloat(),
        targetValue = 2 * size.width.toFloat(),
        animationSpec = infiniteRepeatable(
            animation = tween(1000)
        ), label = "shimmerTransition"
    )

    background(
        brush = Brush.linearGradient(
            colors = listOf(
                Color(0xFFB8B5B5),
                Color(0xFFA5A1A1),
                Color(0xFFB8B5B5),
            ),
            start = Offset(startOffsetX, 0f),
            end = Offset(startOffsetX + size.width.toFloat(), size.height.toFloat())
        )
    )
        .onGloballyPositioned {
            size = it.size
        }
}

fun Modifier.conditional(condition : Boolean, modifier : Modifier.() -> Modifier) : Modifier {
    return if (condition) {
        then(modifier(Modifier))
    } else {
        this
    }
}

fun Modifier.bounceAnimation() = composed {
    val transition = rememberInfiniteTransition(label = "bounce")

    val animationSpec = infiniteRepeatable<Float>(
        animation = tween(
            durationMillis = (500..1000).random(),
            easing = LinearEasing
        ),
        repeatMode = RepeatMode.Reverse
    )

    val offsetY by transition.animateFloat(
        initialValue = 1F,
        targetValue = 4F,
        animationSpec,
        label = "offsetY"
    )

    val bounceScale by transition.animateFloat(
        initialValue = 0.9F,
        targetValue = 1F,
        animationSpec = animationSpec,
        label = "bounceScale"
    )
    this
        .offset(y = offsetY.dp)
        .scale(scaleX = bounceScale, scaleY = 1F)
}

fun Modifier.spin(duration: Int = 1000, delay: Int = 0): Modifier = composed {
    val transition = rememberInfiniteTransition(label = "spin")

    val animationSpec = infiniteRepeatable<Float>(
        animation = tween(
            durationMillis = duration,
            delayMillis = delay,
            easing = EaseInOut
        ),
        repeatMode = RepeatMode.Restart
    )

    val rotation by transition.animateFloat(
        initialValue = 0F,
        targetValue = 360F,
        animationSpec = animationSpec,
        label = "rotation"
    )

    this.graphicsLayer(rotationZ = rotation)
}