package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.user.User

val dummyUsers = mutableListOf(
        User(
            id = "1",
            name = "Alberto",
            username = "alberto_concertina",
            email = "alberto@gmail.com",
            password = "123",
        )
    )