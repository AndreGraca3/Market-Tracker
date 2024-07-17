package pt.isel.markettracker.ui.screens.users.states

import pt.isel.markettracker.domain.model.account.ClientItem

sealed class UsersScreenState {
    data object Idle : UsersScreenState()

    data object Loading : UsersScreenState()

    abstract class Loaded(
        open val users: List<ClientItem>,
        open val hasMore: Boolean
    ) : UsersScreenState()

    data class IdleLoaded(
        override val users: List<ClientItem>,
        override val hasMore: Boolean
    ) : Loaded(users, hasMore)

    data class LoadingMore(
        override val users: List<ClientItem>
    ) : Loaded(users, true)

    data class Failed(val error: Throwable) :
        UsersScreenState()
}

fun UsersScreenState.extractUsers() =
    when (this) {
        is UsersScreenState.Loaded -> users
        else -> emptyList()
    }

fun UsersScreenState.extractHasMore() =
    when(this) {
        is UsersScreenState.Loaded -> hasMore
        else -> false
    }
