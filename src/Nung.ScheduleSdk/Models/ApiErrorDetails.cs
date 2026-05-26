using System.Text.Json.Serialization;
using Nung.ScheduleSdk.Json;

namespace Nung.ScheduleSdk.Models;

/// <summary>
/// Об'єкт помилки у полі <c>error</c> відповіді API.
/// </summary>
public sealed class ApiErrorDetails
{
    /// <summary>
    /// Людсько-читабельний опис помилки українською.
    /// </summary>
    [JsonPropertyName("error_message")]
    public string? Message { get; set; }

    /// <summary>
    /// Числовий код помилки (наприклад <c>-90</c> — об'єкт не знайдено).
    /// </summary>
    [JsonPropertyName("errorcode")]
    [JsonConverter(typeof(FlexibleApiCodeJsonConverter))]
    public int? ErrorCode { get; set; }
}
