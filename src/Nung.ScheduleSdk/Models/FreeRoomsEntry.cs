using System.Text.Json.Serialization;

namespace Nung.ScheduleSdk.Models;

/// <summary>
/// Один запис у відповіді <c>free_rooms_list</c>: дата, номер пари та список вільних аудиторій.
/// </summary>
public sealed class FreeRoomsEntry
{
    /// <summary>Дата у форматі <c>dd.MM.yyyy</c>.</summary>
    [JsonPropertyName("date")]
    public string? Date { get; set; }

    /// <summary>Номер пари.</summary>
    [JsonPropertyName("lesson")]
    public string? Lesson { get; set; }

    /// <summary>Назви вільних аудиторій.</summary>
    [JsonPropertyName("rooms")]
    public IReadOnlyList<string>? Rooms { get; set; }
}
