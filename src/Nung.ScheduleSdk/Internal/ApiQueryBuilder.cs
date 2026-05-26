using System.Globalization;
using Nung.ScheduleSdk.Enums;

namespace Nung.ScheduleSdk.Internal;

/// <summary>
/// Формує query-string для CGI-запитів до <c>timetable_export.cgi</c>.
/// </summary>
internal static class ApiQueryBuilder
{
    /// <summary>Завжди додає <c>req_format=json</c> та <c>coding_mode</c>.</summary>
    public static Dictionary<string, string> CreateBaseQuery(TextEncodingMode encoding)
    {
        return new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["req_format"] = "json",
            ["coding_mode"] = ToEncodingParameter(encoding),
        };
    }

    public static string ToRequestMode(RequestMode mode) =>
        mode switch
        {
            RequestMode.Group => "group",
            RequestMode.Teacher => "teacher",
            RequestMode.Room => "room",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

    public static string ToRequestType(RequestType type) =>
        type switch
        {
            RequestType.ObjectList => "obj_list",
            RequestType.Schedule => "rozklad",
            RequestType.FreeRoomsList => "free_rooms_list",
            RequestType.RoomTypeList => "room_type_list",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };

    public static string ToScheduleTextFormat(ScheduleTextFormat format) =>
        format switch
        {
            ScheduleTextFormat.United => "united",
            ScheduleTextFormat.Separated => "separated",
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null),
        };

    /// <summary>Форматує дату для API: <c>dd.MM.yyyy</c>.</summary>
    public static string FormatApiDate(DateOnly date) =>
        date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

    private static string ToEncodingParameter(TextEncodingMode encoding) =>
        encoding switch
        {
            TextEncodingMode.Utf8 => "UTF8",
            TextEncodingMode.Windows1251 => "WINDOWS-1251",
            _ => throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null),
        };
}
