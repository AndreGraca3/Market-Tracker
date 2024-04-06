package pt.isel.markettracker.ui.theme

import androidx.compose.material3.Typography
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.Font
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.sp
import com.example.markettracker.R

val mainFont = FontFamily(
    Font(R.font.outfit_medium),
    Font(R.font.outfit_bold, FontWeight.W600),
    Font(R.font.outfit_extrabold, FontWeight.W700)
)

// Set of Material typography styles to start with
val Typography = Typography(
    bodyLarge = TextStyle(
        fontFamily = mainFont,
        fontWeight = FontWeight.Normal,
        fontSize = 16.sp,
        color = Color.Black,
        lineHeight = 24.sp,
        letterSpacing = 0.5.sp
    ),
    titleLarge = TextStyle(
        fontFamily = mainFont,
        fontWeight = FontWeight.Normal,
        fontSize = 22.sp,
        color = Color.Black,
        lineHeight = 28.sp,
        letterSpacing = 0.sp
    ),
    labelSmall = TextStyle(
        fontFamily = mainFont,
        fontWeight = FontWeight.Medium,
        fontSize = 11.sp,
        color = Color.Black,
        lineHeight = 16.sp,
        letterSpacing = 0.5.sp
    )
)