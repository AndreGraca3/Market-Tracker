package pt.isel.markettracker.ui.screens.profile

import pt.isel.markettracker.domain.model.account.Client

sealed class ProfileScreenState {
    data object Loading : ProfileScreenState()

    data object Idle : ProfileScreenState()

    data class Success(val client: Client, val nLists: Int, val nFavorites: Int, val nAlerts: Int) :
        ProfileScreenState()

    data class Fail(val error: Throwable) : ProfileScreenState()
}