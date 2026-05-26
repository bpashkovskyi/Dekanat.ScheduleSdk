using System.Text.Json.Serialization;

namespace Nung.ScheduleSdk.Models;

/// <summary>
/// Тип аудиторії з довгою та короткою назвою.
/// </summary>
public sealed class RoomType
{
    /// <summary>Повна назва типу (наприклад «Лекційна»).</summary>
    [JsonPropertyName("full")]
    public string? FullName { get; set; }

    /// <summary>Скорочена назва (наприклад «лек»).</summary>
    [JsonPropertyName("short")]
    public string? ShortName { get; set; }
}
