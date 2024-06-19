package pt.isel.markettracker.repository.auth.GsonSerializer

import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import pt.isel.markettracker.domain.model.CollectionOutputModel
import pt.isel.markettracker.domain.model.PriceAlertOutputModel
import pt.isel.markettracker.http.models.list.ShoppingListOutputModel

object AlertsGsonSerializer : GsonSerializer<CollectionOutputModel<PriceAlertOutputModel>> {
    override fun serialize(data: CollectionOutputModel<PriceAlertOutputModel>): String {
        return Gson().toJson(data)
    }

    override fun deserialize(data: String): CollectionOutputModel<PriceAlertOutputModel> {
        val type = object : TypeToken<CollectionOutputModel<ShoppingListOutputModel>>() {}.type
        return Gson().fromJson(data, type)
    }
}