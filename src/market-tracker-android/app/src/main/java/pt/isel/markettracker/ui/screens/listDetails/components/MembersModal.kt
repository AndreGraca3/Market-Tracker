package pt.isel.markettracker.ui.screens.listDetails.components

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.PersonAdd
import androidx.compose.material3.ModalDrawerSheet
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.RectangleShape
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.components.buttons.MarketTrackerOutlinedButton
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.listDetails.ListDetailsScreenState
import pt.isel.markettracker.ui.screens.listDetails.extractShoppingListSocial
import pt.isel.markettracker.ui.theme.Primary600

@Composable
fun MembersModal(
    state: ListDetailsScreenState,
    onAddUsersToListRequested: () -> Unit,
    onRemoveUserFromLisTRequested: (String) -> Unit,
    modifier: Modifier = Modifier,
) {
    ModalDrawerSheet(
        modifier = modifier
            .fillMaxHeight()
            .fillMaxWidth(0.7F)
            .border(2.dp, color = Color.Black),
        drawerShape = RectangleShape,
        drawerContainerColor = Primary600,
        content = {
            Column {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    when (state) {
                        is ListDetailsScreenState.Loading -> {
                            LoadingIcon()
                        }

                        is ListDetailsScreenState.Failed -> {
                            Text(
                                text = stringResource(id = R.string.failed_to_fetch_users),
                                color = Color.Gray,
                                fontWeight = FontWeight.Bold
                            )
                        }

                        is ListDetailsScreenState.Loaded,
                        -> {
                            val ownerId = state.extractShoppingListSocial()!!.owner.id
                            val users = state.extractShoppingListSocial()!!.members
                            Column {
                                MembersList(
                                    users = users,
                                    ownerId = ownerId,
                                    onRemoveUserFromLisTRequested = onRemoveUserFromLisTRequested,
                                    modifier = Modifier.fillMaxHeight(0.8F),
                                )

                                Box(
                                    modifier = Modifier.fillMaxWidth(),
                                    contentAlignment = Alignment.Center
                                ) {
                                    MarketTrackerOutlinedButton(
                                        text = "Partilhar",
                                        icon = Icons.Default.PersonAdd,
                                        onClick = onAddUsersToListRequested,
                                        modifier = Modifier
                                            .padding(horizontal = 60.dp)
                                    )
                                }
                            }
                        }

                        else -> {}
                    }
                }
            }
        }
    )
}