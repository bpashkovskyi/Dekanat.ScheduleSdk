using System.Text.Json.Serialization;

namespace Dekanat.ScheduleSdk.Models;

/// <summary>
/// Коренева обгортка JSON-відповіді API експорту ПС-Розклад.
/// </summary>
/// <remarks>
/// Усі успішні та помилкові відповіді мають вигляд <c>{ "psrozklad_export": { ... } }</c>.
/// </remarks>
public sealed class PsRozkladResponse
{
    /// <summary>
    /// Вміст експорту: списки об'єктів, розклад, помилки та код результату.
    /// </summary>
    [JsonPropertyName("psrozklad_export")]
    public PsRozkladExport? Export { get; set; }
}
