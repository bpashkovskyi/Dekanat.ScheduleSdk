using System.Text.Json.Serialization;

namespace Nung.ScheduleSdk.Models;

/// <summary>
/// Підрозділ (факультет, інститут, кафедра) з переліком груп або викладачів.
/// </summary>
public sealed class Department
{
    /// <summary>Назва підрозділу.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Групи або викладачі, що належать підрозділу.</summary>
    [JsonPropertyName("objects")]
    public IReadOnlyList<ScheduleEntity>? Objects { get; set; }
}
