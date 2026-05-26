using System.Text.Json.Serialization;

namespace Dekanat.ScheduleSdk.Models;

/// <summary>
/// Об'єкт у переліку: група, викладач або аудиторія.
/// </summary>
/// <remarks>
/// Для викладачів API додатково повертає компоненти ПІБ у полях <see cref="LastName"/>,
/// <see cref="FirstName"/> та <see cref="Patronymic"/>.
/// </remarks>
public sealed class ScheduleEntity
{
    /// <summary>Відображувана назва (назва групи, скорочене ПІБ або назва аудиторії).</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>Ідентифікатор об'єкта в системі ПС-Розклад (лише якщо у запиті було <c>show_ID=yes</c>).</summary>
    [JsonPropertyName("ID")]
    public string? Id { get; set; }

    /// <summary>Прізвище викладача (поле <c>P</c> у JSON).</summary>
    [JsonPropertyName("P")]
    public string? LastName { get; set; }

    /// <summary>Ім'я викладача (поле <c>I</c> у JSON).</summary>
    [JsonPropertyName("I")]
    public string? FirstName { get; set; }

    /// <summary>По батькові викладача (поле <c>B</c> у JSON).</summary>
    [JsonPropertyName("B")]
    public string? Patronymic { get; set; }
}
