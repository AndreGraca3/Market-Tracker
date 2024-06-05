package pt.isel.markettracker.ui.screens.profile

import pt.isel.markettracker.domain.IOState
import pt.isel.markettracker.domain.model.account.Client

sealed class ProfileScreenState {
    data object Loading : ProfileScreenState()

    data object Idle : ProfileScreenState()

    data class Fail(val error: Throwable) : ProfileScreenState()

    data class Loaded(val client: Client) : ProfileScreenState()
}