using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Pipeline;

public class OptionalConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }
        if (typeToConvert.GetGenericTypeDefinition() != typeof(Optional<>))
        {
            return false;
        }
        return true;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type valueType = typeToConvert.GetGenericArguments()[0];

        return (JsonConverter)
            Activator.CreateInstance(
                type: typeof(OptionalConverterInner<>).MakeGenericType(new Type[] { valueType }),
                bindingAttr: BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null
            )!;
    }

    private class OptionalConverterInner<T> : JsonConverter<Optional<T>>
    {
        public override Optional<T> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            T? value = JsonSerializer.Deserialize<T>(ref reader, options);
            return new Optional<T>(value);
        }

        public override void Write(
            Utf8JsonWriter writer,
            Optional<T> value,
            JsonSerializerOptions options
        ) => JsonSerializer.Serialize(writer, value.Value, options);
    }
}
