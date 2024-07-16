package pt.isel.markettracker.ui.screens.listDetails.cards

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.IconButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.domain.model.account.ClientItem
import pt.isel.markettracker.ui.theme.mainFont

@Composable
fun UserCard(
    user: ClientItem,
    isOwner: Boolean,
    userToListRequested: (String) -> Unit,
) {
    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier
            .size(width = 220.dp, height = 60.dp)
    ) {
        Card(
            modifier = Modifier
                .fillMaxSize()
                .border(2.dp, color = Color.Black)
                .clip(RoundedCornerShape(8.dp))
                .background(color = Color.White)

        ) {
            Box(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(2.dp),
                contentAlignment = Alignment.Center
            ) {
                Row(
                    horizontalArrangement = Arrangement.Center,
                    modifier = Modifier.fillMaxSize()
                ) {
                    Box(
                        modifier = Modifier.fillMaxHeight(),
                        contentAlignment = Alignment.Center
                    ) {
                        Image(
                            painter = painterResource(id = R.drawable.user_icon),
                            contentDescription = null,
                        )
                    }
                    Box(
                        contentAlignment = Alignment.Center,
                        modifier = Modifier
                            .fillMaxHeight()
                            .fillMaxWidth(0.7F)
                    ) {
                        Text(
                            text = user.username,
                            fontFamily = mainFont,
                            textAlign = TextAlign.Center,
                        )
                    }
                    Box(
                        modifier = Modifier.fillMaxHeight(),
                        contentAlignment = Alignment.Center
                    ) {
                        if (isOwner) {
                            Image(
                                painter = painterResource(id = R.drawable.crown),
                                contentDescription = "",
                                modifier = Modifier
                                    .size(30.dp)
                            )
                        } else {
                            IconButton(
                                onClick = { userToListRequested(user.id) }
                            ) {
                                Box(
                                    modifier = Modifier.fillMaxSize(),
                                    contentAlignment = Alignment.Center
                                ) {
                                    Image(
                                        painter = painterResource(id = R.drawable.person_remove),
                                        contentDescription = "",
                                        modifier = Modifier
                                            .size(30.dp)
                                    )
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}