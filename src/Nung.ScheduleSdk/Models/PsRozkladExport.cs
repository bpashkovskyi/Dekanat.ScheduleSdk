using System.Text.Json.Serialization;
using Nung.ScheduleSdk.Json;

namespace Nung.ScheduleSdk.Models;

/// <summary>
/// Тіло відповіді всередині <see cref="PsRozkladResponse.Export"/>.
/// </summary>
/// <remarks>
/// Набір заповнених полів залежить від <c>req_type</c>: для <c>obj_list</c> — <see cref="Departments"/> або
/// <see cref="Blocks"/>, для <c>rozklad</c> — <see cref="ScheduleItems"/>, для <c>room_type_list</c> — <see cref="RoomTypes"/>.
/// </remarks>
public sealed class PsRozkladExport
{
    /// <summary>
    /// Підрозділи з переліком об'єктів (групи по факультетах, викладачі по кафедрах).
    /// </summary>
    /// <remarks>Присутнє для <c>req_mode=group|teacher</c> та <c>req_type=obj_list</c>.</remarks>
    [JsonPropertyName("departments")]
    public IReadOnlyList<Department>? Departments { get; set; }

    /// <summary>
    /// Корпуси (блоки) з переліком аудиторій.
    /// </summary>
    /// <remarks>Присутнє для <c>req_mode=room</c> та <c>req_type=obj_list</c>.</remarks>
    [JsonPropertyName("blocks")]
    public IReadOnlyList<Building>? Blocks { get; set; }

    /// <summary>
    /// Елементи розкладу (пари) за обраний період.
    /// </summary>
    /// <remarks>Присутнє для <c>req_type=rozklad</c>.</remarks>
    [JsonPropertyName("roz_items")]
    public IReadOnlyList<ScheduleItem>? ScheduleItems { get; set; }

    /// <summary>
    /// Типи аудиторій закладу.
    /// </summary>
    /// <remarks>Присутнє для <c>req_type=room_type_list</c>.</remarks>
    [JsonPropertyName("objects")]
    public IReadOnlyList<RoomType>? RoomTypes { get; set; }

    /// <summary>
    /// Записи про вільні аудиторії (може бути кілька за один запит).
    /// </summary>
    /// <remarks>Присутнє для <c>req_type=free_rooms_list</c>.</remarks>
    [JsonPropertyName("free_rooms")]
    public IReadOnlyList<FreeRoomsEntry>? FreeRooms { get; set; }

    /// <summary>
    /// Деталі помилки, якщо запит завершився невдало.
    /// </summary>
    [JsonPropertyName("error")]
    public ApiErrorDetails? Error { get; set; }

    /// <summary>
    /// Код результату операції на сервері (рядок, наприклад <c>"0"</c>).
    /// </summary>
    [JsonPropertyName("code")]
    [JsonConverter(typeof(FlexibleApiCodeJsonConverter))]
    public int? Code { get; set; }

    /// <summary>
    /// Перевіряє, чи відповідь позначена сервером як успішна (<c>code == 0</c>).
    /// </summary>
    public bool IsSuccess => Code is 0 or null && Error is null;

    /// <summary>
    /// Повертає нормалізований код помилки API (з урахуванням <see cref="ApiErrorDetails.ErrorCode"/>).
    /// </summary>
    public int? GetNormalizedErrorCode()
    {
        if (Error?.ErrorCode is not null)
        {
            return Error.ErrorCode;
        }

        if (Code is > 0)
        {
            // Сервер інколи повертає code=90 замість errorcode=-90.
            return -Code.Value;
        }

        return Code;
    }
}
