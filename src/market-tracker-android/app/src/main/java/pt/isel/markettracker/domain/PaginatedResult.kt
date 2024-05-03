package pt.isel.markettracker.domain

data class PaginatedResult<T>(
    val items: List<T>,
    val currentPage: Int,
    val itemsPerPage: Int,
    val totalItems: Int,
    val totalPages: Int,
    val hasMore: Boolean = currentPage < totalPages
)
