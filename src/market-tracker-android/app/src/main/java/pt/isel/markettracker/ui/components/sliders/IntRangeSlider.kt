package pt.isel.markettracker.ui.components.sliders

import androidx.compose.material3.RangeSlider
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue

@Composable
fun IntRangeSlider(
    sliderPosition: ClosedFloatingPointRange<Float>,
    onValueChange: (ClosedFloatingPointRange<Float>) -> Unit,
    onValueChangeFinished: (IntRange) -> Unit,
    enabled: Boolean = true
) {
    RangeSlider(
        value = sliderPosition,
        onValueChange = onValueChange,
        valueRange = 0f..100f,
        onValueChangeFinished = {
            onValueChangeFinished(sliderPosition.toIntRange())
        },
        enabled = enabled
    )
}

private fun ClosedFloatingPointRange<Float>.toIntRange(): IntRange =
    this.start.toInt()..this.endInclusive.toInt()