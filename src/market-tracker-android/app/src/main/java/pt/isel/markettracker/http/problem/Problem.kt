package pt.isel.markettracker.http.problem

import okhttp3.MediaType.Companion.toMediaType
import java.time.LocalDateTime
import java.util.UUID

open class Problem(
    val id: String,
    val type: String,
    val title: String,
    val status: Int,
    val detail: String,
    val timestamp: LocalDateTime
) {
    companion object {
        const val BASE_TYPE = "https://markettracker.pt/probs/"

        val MEDIA_TYPE = "application/problem+json".toMediaType() // TODO: Voltar a colocar o UTF-8
    }

    override fun toString(): String {
        return "{type=$type, title=$title, status=$status, detail=$detail}"
    }
}

class InternalServerErrorProblem : Problem(
    id = UUID.randomUUID().toString(),
    type = "${BASE_TYPE}internal-server-error",
    title = "Something went wrong",
    status = 500,
    detail = "Something went wrong, please try again later",
    timestamp = LocalDateTime.now()
)