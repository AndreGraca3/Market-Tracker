package pt.isel.markettracker.ui.screens.products.topbar

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import com.example.markettracker.R
import pt.isel.markettracker.ui.theme.Primary700

@Composable
fun HeaderLogo(modifier: Modifier = Modifier) {
    Box(
        contentAlignment = Alignment.Center,
        modifier = modifier
            .background(Color.White, CircleShape)
    ) {
        Image(
            painter = painterResource(id = R.drawable.mt_logo), contentDescription = "",
            modifier = Modifier
                .padding(2.dp)
        )
    }
}

@Preview
@Composable
fun HeaderLogoPreview() {
    HeaderLogo()
}
