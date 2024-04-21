package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.user.User

val dummyUsers = mutableListOf(
        User(
            id = "1",
            name = "Diogo",
            username = "Digo",
            email = "Diogo@gmail.com",
            password = "123",
        )
    )