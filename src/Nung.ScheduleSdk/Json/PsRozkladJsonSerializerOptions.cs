using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nung.ScheduleSdk.Json;

/// <summary>
/// Налаштування <see cref="JsonSerializer"/> для відповідей API ПС-Розклад.
/// </summary>
public static class PsRozkladJsonSerializerOptions
{
    /// <summary>
    /// Опції за замовчуванням: нечутливість до регістру вимкнена (імена полів збігаються з API),
    /// дозволені коментарі у trailing commas не потрібні — API повертає компактний JSON.
    /// </summary>
    public static JsonSerializerOptions Default { get; } = Create();

    /// <summary>Створює новий екземпляр опцій серіалізації SDK.</summary>
    public static JsonSerializerOptions Create()
    {
        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = false,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        options.Converters.Add(new FlexibleApiCodeJsonConverter());
        return options;
    }
}
