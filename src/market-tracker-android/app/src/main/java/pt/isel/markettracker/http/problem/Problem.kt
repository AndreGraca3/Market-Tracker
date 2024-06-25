package pt.isel.markettracker.http.problem

import okhttp3.MediaType.Companion.toMediaType
import java.time.LocalDateTime

open class Problem(
    val Id: String,
    val Type: String,
    val Title: String,
    val Status: Int,
    val Detail: String,
    val Timestamp: LocalDateTime,
    val Data: String?
) {
    companion object {
        const val BASE_TYPE = "https://markettracker.pt/probs/"

        val MEDIA_TYPE = "application/problem+json".toMediaType()
    }

    override fun toString(): String {
        return "{type=$Type, title=$Title, status=$Status, detail=$Detail}"
    }
}

class InternalServerErrorProblem : Problem(
    Id = "549492bb-6c59-495a-a274-693ff05e83c9",
    Type = "${BASE_TYPE}internal-server-error",
    Title = "Something went wrong",
    Status = 500,
    Detail = "Something went wrong, please try again later",
    Timestamp = LocalDateTime.now(),
    Data = null
)