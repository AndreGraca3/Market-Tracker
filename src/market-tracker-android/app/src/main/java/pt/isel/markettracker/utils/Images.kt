package pt.isel.markettracker.utils

import android.content.ContentResolver
import android.graphics.BitmapFactory
import android.net.Uri
import android.util.Base64
import java.io.InputStream

fun convertImageToBase64(contentResolver: ContentResolver, uri: Uri): String? {

    return try {
        val inputStream: InputStream? = contentResolver.openInputStream(uri)
        val bytes: ByteArray = inputStream?.readBytes() ?: return null

        // Convert the bytes to base64
        val base64Image: String = Base64.encodeToString(bytes, Base64.DEFAULT)

        inputStream.close()
        val mimeType = contentResolver.getType(uri)
        "data:$mimeType;base64,$base64Image"
    } catch (e: Exception) {
        e.printStackTrace()
        null
    }
}

fun convertBase64ToImage(base64String: String): Uri {
   return Uri.parse(Base64.decode(base64String, Base64.DEFAULT).toString())
}