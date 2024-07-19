package pt.isel.markettracker.ui.screens.favorites.list

import android.util.Log
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import pt.isel.markettracker.R
import pt.isel.markettracker.ui.components.common.LoadingIcon
import pt.isel.markettracker.ui.screens.favorites.FavoriteScreenState
import pt.isel.markettracker.ui.screens.favorites.card.FavoriteProductCard
import pt.isel.markettracker.ui.screens.favorites.extractFavorites

@Composable
fun FavoriteListView(
    state: FavoriteScreenState,
    onDeleteProductFromFavoritesRequested: (String) -> Unit,
) {
    when (state) {
        is FavoriteScreenState.Loaded -> {
            val favorites = state.extractFavorites()
            if (favorites.isEmpty()) {
                Column(
                    horizontalAlignment = Alignment.CenterHorizontally,
                ) {
                    Image(
                        painter = painterResource(id = R.drawable.products_not_found),
                        contentDescription = "No products found"
                    )
                    Text(
                        text = stringResource(id = R.string.favorites_not_found)
                    )
                }
            } else {
                Box(
                    modifier = Modifier
                        .fillMaxWidth(),
                    contentAlignment = Alignment.TopCenter
                ) {
                    LazyColumn(
                        verticalArrangement = Arrangement.spacedBy(10.dp),
                        contentPadding = PaddingValues(horizontal = 16.dp, vertical = 12.dp),
                    ) {
                        Log.v("Favorites", "Favorites are $favorites")
                        items(favorites.size) { index ->
                            FavoriteProductCard(
                                product = favorites[index],
                                onRemoveFromFavoritesRequested = {
                                    onDeleteProductFromFavoritesRequested(favorites[index].productId)
                                }
                            )
                        }
                    }
                }
            }
        }

        is FavoriteScreenState.Loading -> {
            LoadingIcon(text = "Carregando os Favoritos...")
        }


        else -> {
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
            ) {
                Image(
                    painter = painterResource(id = R.drawable.server_error),
                    contentDescription = "No products found"
                )
                Text(
                    text = stringResource(id = R.string.loading_error)
                )
            }
        }
    }
}