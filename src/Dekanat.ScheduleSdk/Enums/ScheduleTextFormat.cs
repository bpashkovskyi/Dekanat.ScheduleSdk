namespace Dekanat.ScheduleSdk.Enums;

/// <summary>
/// Значення параметра <c>ros_text</c> для запитів розкладу (<c>req_type=rozklad</c>).
/// </summary>
public enum ScheduleTextFormat
{
    /// <summary>
    /// Сформований текст у полі <c>lesson_description</c> (режим за замовчуванням на сервері).
    /// </summary>
    United,

    /// <summary>
    /// Окремі стовпчики: викладач, аудиторія, назва дисципліни тощо.
    /// </summary>
    Separated,
}
