package pt.isel.markettracker.http.problem

import okhttp3.MediaType.Companion.toMediaType
import java.time.LocalDateTime
import java.util.UUID

open class Problem(
    val Id: String,
    val Type: String,
    val Title: String,
    val Status: Int,
    val Detail: String,
    val Timestamp: LocalDateTime
) {
    companion object {
        const val BASE_TYPE = "https://markettracker.pt/probs/"

        val MEDIA_TYPE = "application/problem+json".toMediaType() // TODO: Voltar a colocar o UTF-8
    }

    override fun toString(): String {
        return "{type=$Type, title=$Title, status=$Status, detail=$Detail}"
    }
}

class InternalServerErrorProblem : Problem(
    Id = UUID.randomUUID().toString(),
    Type = "${BASE_TYPE}internal-server-error",
    Title = "Something went wrong",
    Status = 500,
    Detail = "Something went wrong, please try again later",
    Timestamp = LocalDateTime.now()
)