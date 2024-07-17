package pt.isel.markettracker.ui.screens.favorites

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.hilt.navigation.compose.hiltViewModel

@Composable
fun ListFavoritesScreen(
    onBackRequested: () -> Unit,
    favoritesScreenViewModel: FavoritesScreenViewModel = hiltViewModel(),
) {
    val favoritesState by favoritesScreenViewModel.favorites.collectAsState()

    LaunchedEffect(Unit) {
        favoritesScreenViewModel.fetchFavorites()
    }

    ListFavoritesScreenView(
        state = favoritesState,
        onRemoveFromFavoritesRequested = {
            favoritesScreenViewModel.removeFromFavorites(it)
        },
        onBackRequested = onBackRequested
    )
}