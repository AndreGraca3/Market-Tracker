package pt.isel.markettracker.http.models.user

import pt.isel.markettracker.domain.user.User

data class UsersOutputModel(val users: List<UserOutputModel>, val total: Int)
