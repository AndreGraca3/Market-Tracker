package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.model.account.User
import java.time.LocalDateTime

val dummyUsers = mutableListOf(
        User(
            id = "1",
            name = "Alberto",
            username = "alberto_concertina",
            email = "alberto@gmail.com",
            password = "123",
            createdAt = LocalDateTime.now()
        )
    )