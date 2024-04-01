package pt.isel.markettracker.http.problem

data class Problem(
    val type: String,
    val title: String,
    val status: Int,
    val detail: String,
    // TODO val data: T
) {
    companion object {
        val INTERNAL_SERVER_ERROR = Problem(
            type = "https://markettracker.pt/probs/internal-server-error",
            title = "Something went wrong",
            status = 500,
            detail = "Something went wrong, please try again later."
        )
    }

    override fun toString(): String {
        return "{type=$type, title=$title, status=$status, detail=$detail}"
    }
}