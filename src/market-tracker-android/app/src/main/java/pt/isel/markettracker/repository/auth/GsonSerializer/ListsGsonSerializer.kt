package pt.isel.markettracker.repository.auth.GsonSerializer

import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.http.models.list.ShoppingListOutputModel

object ListsGsonSerializer : GsonSerializer<CollectionOutputModel<ShoppingListOutputModel>> {
    override fun serialize(data: CollectionOutputModel<ShoppingListOutputModel>): String {
        return Gson().toJson(data)
    }

    override fun deserialize(data: String): CollectionOutputModel<ShoppingListOutputModel> {
        val type = object : TypeToken<CollectionOutputModel<ShoppingListOutputModel>>() {}.type
        return Gson().fromJson(data, type)
    }
}