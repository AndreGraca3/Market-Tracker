package pt.isel.markettracker.repository.auth.GsonSerializer

interface GsonSerializer<T> {
    fun serialize(data: T): String
    fun deserialize(data: String): T
}