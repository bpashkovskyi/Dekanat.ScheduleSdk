using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.Models;
using Dekanat.ScheduleSdk.Requests;

namespace Dekanat.ScheduleSdk;

/// <summary>
/// Клієнт JSON API експорту розкладу ПС-Розклад (НУГ).
/// </summary>
/// <seealso href="https://dekanat.nung.edu.ua/cgi-bin/timetable_export.cgi">Офіційна документація API</seealso>
public interface IPsRozkladClient
{
    /// <summary>
    /// Отримує перелік груп, викладачів або аудиторій (<c>req_type=obj_list</c>).
    /// </summary>
    Task<PsRozkladExport> GetObjectListAsync(
        ObjectListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Отримує розклад для групи, викладача або аудиторії (<c>req_type=rozklad</c>).
    /// </summary>
    Task<PsRozkladExport> GetScheduleAsync(
        ScheduleRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Отримує перелік вільних аудиторій (<c>req_type=free_rooms_list</c>).
    /// </summary>
    Task<PsRozkladExport> GetFreeRoomsAsync(
        FreeRoomsListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Отримує перелік типів аудиторій (<c>req_type=room_type_list</c>).
    /// </summary>
    Task<PsRozkladExport> GetRoomTypesAsync(
        RoomTypeListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Виконує довільний запит і повертає повну десеріалізовану відповідь (низькорівневий метод).
    /// </summary>
    Task<PsRozkladResponse> SendAsync(
        IReadOnlyDictionary<string, string> queryParameters,
        TextEncodingMode? encodingOverride = null,
        CancellationToken cancellationToken = default);
}
