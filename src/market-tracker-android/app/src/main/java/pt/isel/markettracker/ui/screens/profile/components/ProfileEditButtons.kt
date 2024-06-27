package pt.isel.markettracker.ui.screens.profile.components

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier

@Composable
fun ProfileEditButtons(
    onSaveChangesRequested: () -> Unit,
    onCancelChangesRequested: () -> Unit
) {
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
}