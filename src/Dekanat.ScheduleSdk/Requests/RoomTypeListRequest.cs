using Dekanat.ScheduleSdk.Enums;

namespace Dekanat.ScheduleSdk.Requests;

/// <summary>
/// Параметри запиту переліку типів аудиторій (<c>req_type=room_type_list</c>).
/// </summary>
public sealed class RoomTypeListRequest
{
    /// <summary>Перевизначення кодування для цього запиту.</summary>
    public TextEncodingMode? Encoding { get; init; }
}
