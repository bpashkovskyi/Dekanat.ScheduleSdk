using System.Text.Json.Serialization;

namespace Dekanat.ScheduleSdk.Models;

/// <summary>
/// Один слот розкладу (пара) у масиві <c>roz_items</c>.
/// </summary>
/// <remarks>
/// При <c>ros_text=united</c> основний текст заняття знаходиться в <see cref="LessonDescription"/>.
/// При <c>ros_text=separated</c> заповнюються окремі поля <see cref="Teacher"/>, <see cref="Room"/>, <see cref="Title"/> тощо.
/// Обидва набори полів можуть бути присутні в одному об'єкті залежно від налаштувань сервера.
/// </remarks>
public sealed class ScheduleItem
{
    /// <summary>Назва об'єкта, для якого будується розклад (група, ПІБ викладача, аудиторія).</summary>
    [JsonPropertyName("object")]
    public string? ObjectName { get; set; }

    /// <summary>Дата заняття у форматі <c>dd.MM.yyyy</c>.</summary>
    [JsonPropertyName("date")]
    public string? Date { get; set; }

    /// <summary>Службовий коментар дня (часто <c>"0"</c> або <c>"1"</c>).</summary>
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    /// <summary>Номер пари.</summary>
    [JsonPropertyName("lesson_number")]
    public string? LessonNumber { get; set; }

    /// <summary>Назва / підпис пари (зазвичай збігається з номером).</summary>
    [JsonPropertyName("lesson_name")]
    public string? LessonName { get; set; }

    /// <summary>Часовий інтервал пари, наприклад <c>08:00-09:20</c>.</summary>
    [JsonPropertyName("lesson_time")]
    public string? LessonTime { get; set; }

    /// <summary>
    /// Сформований текст опису заняття (режим <c>ros_text=united</c>).
    /// </summary>
    [JsonPropertyName("lesson_description")]
    public string? LessonDescription { get; set; }

    /// <summary>Половина пари (якщо пара розбита).</summary>
    [JsonPropertyName("half")]
    public string? Half { get; set; }

    /// <summary>Основний викладач (режим <c>ros_text=separated</c>).</summary>
    [JsonPropertyName("teacher")]
    public string? Teacher { get; set; }

    /// <summary>Додаткові викладачі.</summary>
    [JsonPropertyName("teachers_add")]
    public string? AdditionalTeachers { get; set; }

    /// <summary>Аудиторія.</summary>
    [JsonPropertyName("room")]
    public string? Room { get; set; }

    /// <summary>Група / потік / збірна група.</summary>
    [JsonPropertyName("group")]
    public string? Group { get; set; }

    /// <summary>Назва дисципліни.</summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>Тип заняття (Л, Пр, Лаб тощо).</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>Інформація про заміну або відміну.</summary>
    [JsonPropertyName("replacement")]
    public string? Replacement { get; set; }

    /// <summary>Резервування аудиторії.</summary>
    [JsonPropertyName("reservation")]
    public string? Reservation { get; set; }

    /// <summary>Ознака онлайн-заняття (наприклад <c>Так</c>).</summary>
    [JsonPropertyName("online")]
    public string? Online { get; set; }

    /// <summary>Коментар для посилання.</summary>
    [JsonPropertyName("comment4link")]
    public string? LinkComment { get; set; }

    /// <summary>Посилання на онлайн-заняття.</summary>
    [JsonPropertyName("link")]
    public string? Link { get; set; }
}
