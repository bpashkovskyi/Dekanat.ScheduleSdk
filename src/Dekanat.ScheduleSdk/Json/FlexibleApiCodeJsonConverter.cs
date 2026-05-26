using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dekanat.ScheduleSdk.Json;

/// <summary>
/// Десеріалізує коди API, які сервер інколи повертає як рядок (<c>"0"</c>, <c>"-90"</c>) або число.
/// </summary>
internal sealed class FlexibleApiCodeJsonConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.Number when reader.TryGetInt32(out int number) => number,
            JsonTokenType.String => ParseString(reader.GetString()),
            _ => throw new JsonException($"Непідтримуваний тип токена для коду API: {reader.TokenType}."),
        };
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteNumberValue(value.Value);
    }

    private static int? ParseString(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed))
        {
            return parsed;
        }

        throw new JsonException($"Не вдалося розпарсити код API: '{text}'.");
    }
}
