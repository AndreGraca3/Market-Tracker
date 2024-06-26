package pt.isel.markettracker.ui.screens.profile.components

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.CameraAlt
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.unit.dp
import coil.compose.SubcomposeAsyncImage
import pt.isel.markettracker.R

const val AvatarTag = "ProfileScreenAvatarTag"

@Composable
fun AsyncAvatarIcon(
    avatarIcon: String?,
    isEditing: Boolean,
    onIconClick: () -> Unit,
) {
    Box {
        SubcomposeAsyncImage(
            contentScale = ContentScale.Crop,
            model = avatarIcon ?: R.drawable.user_icon,
            contentDescription = null,
            modifier = Modifier
                .size(150.dp)
                .border(2.dp, Color.Red, CircleShape)
                .clip(CircleShape)
                .testTag(AvatarTag)
        )

        if (isEditing) {
            IconButton(
                onClick = onIconClick,
                modifier = Modifier.align(alignment = Alignment.BottomEnd)
            ) {
                Icon(
                    imageVector = Icons.Default.CameraAlt,
                    contentDescription = "settings_icon"
                )
            }
        }
    }
}