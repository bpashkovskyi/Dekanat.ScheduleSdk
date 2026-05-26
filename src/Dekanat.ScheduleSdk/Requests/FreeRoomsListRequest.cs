using Dekanat.ScheduleSdk.Enums;

namespace Dekanat.ScheduleSdk.Requests;

/// <summary>
/// Параметри запиту вільних аудиторій (<c>req_type=free_rooms_list</c>).
/// </summary>
public sealed class FreeRoomsListRequest
{
    /// <summary>Дата пошуку вільних аудиторій.</summary>
    public required DateOnly Date { get; init; }

    /// <summary>Номер пари (1–8 залежно від налаштувань закладу).</summary>
    public required int LessonNumber { get; init; }

    /// <summary>Назва корпусу (<c>block_name</c>).</summary>
    public string? BuildingName { get; init; }

    /// <summary>Тип аудиторії (<c>room_type</c>).</summary>
    public string? RoomType { get; init; }

    /// <summary>Мінімальна кількість місць (<c>size_min</c>).</summary>
    public int? MinimumCapacity { get; init; }

    /// <summary>Максимальна кількість місць (<c>size_max</c>).</summary>
    public int? MaximumCapacity { get; init; }

    /// <summary>Перевизначення кодування для цього запиту.</summary>
    public TextEncodingMode? Encoding { get; init; }
}
