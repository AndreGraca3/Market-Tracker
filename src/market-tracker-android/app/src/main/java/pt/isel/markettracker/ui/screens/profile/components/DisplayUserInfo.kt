package pt.isel.markettracker.ui.screens.profile.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.LocalTextStyle
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import pt.isel.markettracker.ui.components.text.MarketTrackerTextField
import pt.isel.markettracker.ui.theme.mainFont
import java.time.LocalDateTime

@Composable
fun DisplayUserInfo(
    name: String,
    username: String,
    email: String,
    createdAt: LocalDateTime,
    onNameChangeRequested: (String) -> Unit,
    onUsernameChangeRequested: (String) -> Unit,
    onSaveChangesRequested: () -> Unit,
    onCancelChangesRequested: () -> Unit,
    isInEditMode: Boolean
) {
    if (isInEditMode) {
        MarketTrackerTextField(
            value = name,
            onValueChange = onNameChangeRequested,
            textStyle = LocalTextStyle.current.copy(textAlign = TextAlign.Center)
        )

        MarketTrackerTextField(
            value = username,
            onValueChange = onUsernameChangeRequested,
            textStyle = LocalTextStyle.current.copy(textAlign = TextAlign.Center)
        )

        Text(
            text = email,
            fontFamily = mainFont,
            fontSize = 20.sp
        )
    } else {
        Text(
            text = name,
            fontFamily = mainFont,
            fontSize = 20.sp
        )

        Text(
            text = username,
            fontFamily = mainFont,
            fontSize = 20.sp
        )

        Text(
            text = email,
            fontFamily = mainFont,
            fontSize = 20.sp
        )
    }

    Box(
        modifier = Modifier
            .fillMaxSize()
            .padding(vertical = 10.dp),
        contentAlignment = Alignment.BottomCenter
    ) {
        if (isInEditMode) {
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.Center
            ) {
                Box(
                    modifier = Modifier
                        .weight(0.5F),
                    contentAlignment = Alignment.Center
                ) {
                    Button(
                        onClick = onSaveChangesRequested
                    ) {
                        Text(
                            text = "Guardar ✔️"
                        )
                    }
                }

                Box(
                    modifier = Modifier
                        .weight(0.5F),
                    contentAlignment = Alignment.Center
                ) {
                    Button(
                        onClick = onCancelChangesRequested
                    ) {
                        Text(
                            text = "Cancelar ❌"
                        )
                    }
                }
            }
        } else {
            TimeDisplay(createdAt)
        }
    }
}