package pt.isel.markettracker.http.models

data class IdOutputModel(val id: String) {
    constructor(id: Int): this(id.toString())
}