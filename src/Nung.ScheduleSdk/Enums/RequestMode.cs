namespace Nung.ScheduleSdk.Enums;

/// <summary>
/// Значення параметра <c>req_mode</c> — визначає тип об'єктів експорту (групи, викладачі або аудиторії).
/// </summary>
/// <seealso href="https://dekanat.nung.edu.ua/cgi-bin/timetable_export.cgi">Документація API</seealso>
public enum RequestMode
{
    /// <summary>Експорт груп (факультети / інститути).</summary>
    Group,

    /// <summary>Експорт викладачів (кафедри).</summary>
    Teacher,

    /// <summary>Експорт аудиторій (корпуси / блоки).</summary>
    Room,
}
