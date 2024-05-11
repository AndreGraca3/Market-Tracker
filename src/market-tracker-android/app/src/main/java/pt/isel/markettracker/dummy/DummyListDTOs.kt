package pt.isel.markettracker.dummy

import pt.isel.markettracker.domain.list.ListInfo
import java.time.LocalDateTime
import java.util.UUID

val dummyList = listOf(
    ListInfo(
        1,
        "Festa de aniversário",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
        0
    ),
    ListInfo(
        2,
        "Ano Novo",
        null,
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
        1
    ),
    ListInfo(
        3,
        "Compras Semanais",
        null,
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
        1
    ),
    ListInfo(
        4,
        "Compras Mensais",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
        1
    ),
    ListInfo(
        5,
        "Presentes de Natal",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
        2
    ),
    ListInfo(
        6,
        "Aperitivos para a festa",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
        3
    ),
    ListInfo(
        7,
        "Coisas que provavelmente não vou comprar",
        LocalDateTime.now().minusDays(1),
        LocalDateTime.now().minusDays(2),
        UUID(1, 1),
        4
    )
)