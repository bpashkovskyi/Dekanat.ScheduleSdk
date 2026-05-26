namespace Dekanat.ScheduleSdk.Enums;

/// <summary>
/// Значення параметра <c>req_type</c> — визначає вид даних у відповіді API.
/// </summary>
/// <seealso href="https://dekanat.nung.edu.ua/cgi-bin/timetable_export.cgi">Документація API</seealso>
public enum RequestType
{
    /// <summary>Перелік об'єктів (груп, викладачів або аудиторій) згрупований по підрозділах.</summary>
    ObjectList,

    /// <summary>Розклад занять для обраного об'єкта за період дат.</summary>
    Schedule,

    /// <summary>Перелік вільних аудиторій на дату та номер пари.</summary>
    FreeRoomsList,

    /// <summary>Перелік типів аудиторій, налаштованих у закладі.</summary>
    RoomTypeList,
}
