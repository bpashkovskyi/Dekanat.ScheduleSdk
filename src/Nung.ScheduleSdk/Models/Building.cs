using System.Text.Json.Serialization;

namespace Nung.ScheduleSdk.Models;

/// <summary>
/// Корпус (блок) з переліком аудиторій.
/// </summary>
/// <remarks>Відповідає масиву <c>blocks</c> у JSON для <c>req_mode=room</c>.</remarks>
public sealed class Building
{
    /// <summary>Назва корпусу / блоку (наприклад <c>"0"</c>, <c>"5"</c>).</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Аудиторії в цьому корпусі.</summary>
    [JsonPropertyName("objects")]
    public IReadOnlyList<ScheduleEntity>? Objects { get; set; }
}
