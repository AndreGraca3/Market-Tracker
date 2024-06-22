package pt.isel.markettracker.http.problem

import okhttp3.MediaType.Companion.toMediaType

open class Problem(
    subType: String,
    val title: String,
    val status: Int,
    val detail: String
) {
    companion object {
        const val BASE_TYPE = "https://markettracker.pt/probs/"

        val MEDIA_TYPE = "application/problem+json".toMediaType()
    }

    val type: String = BASE_TYPE + subType

    override fun toString(): String {
        return "{type=$type, title=$title, status=$status, detail=$detail}"
    }
}

class InternalServerErrorProblem : Problem(
    subType = "internal-server-error",
    title = "Something went wrong",
    status = 500,
    detail = "Something went wrong, please try again later"
)