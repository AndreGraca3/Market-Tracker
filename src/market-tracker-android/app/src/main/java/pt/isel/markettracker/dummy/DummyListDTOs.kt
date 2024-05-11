package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.list.ListInfo
import java.time.LocalDateTime
import java.util.UUID

val dummyList = listOf(
    ListInfo(
        1,
        "List 1",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
    ),
    ListInfo(
        2,
        "List 2",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
    ),
    ListInfo(
        3,
        "List 3",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
    ),
    ListInfo(
        4,
        "List 4",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
    ),
    ListInfo(
        5,
        "List 5",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
    ),
    ListInfo(
        6,
        "List 6",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
    ),
    ListInfo(
        7,
        "List 7",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
    )
)