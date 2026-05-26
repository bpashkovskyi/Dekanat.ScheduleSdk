using Nung.ScheduleSdk.Enums;

namespace Nung.ScheduleSdk.Requests;

/// <summary>
/// Параметри запиту розкладу (<c>req_type=rozklad</c>).
/// </summary>
/// <remarks>
/// Потрібно вказати рівно один спосіб ідентифікації об'єкта: <see cref="ObjectId"/>, <see cref="ObjectName"/>
/// або <see cref="DepartmentName"/> (останній — лише розклад на один день).
/// </remarks>
public sealed class ScheduleRequest
{
    /// <summary>Режим: група, викладач або аудиторія.</summary>
    public required RequestMode Mode { get; init; }

    /// <summary>Ідентифікатор об'єкта (<c>OBJ_ID</c>).</summary>
    public string? ObjectId { get; init; }

    /// <summary>Точна назва групи, ПІБ або аудиторії (<c>OBJ_name</c>).</summary>
    public string? ObjectName { get; init; }

    /// <summary>
    /// Назва підрозділу (<c>dep_name</c>), якщо ID та назва об'єкта не задані.
    /// Значення <c>all</c> — весь розклад закладу (потрібні права адміністратора).
    /// </summary>
    public string? DepartmentName { get; init; }

    /// <summary>Дата початку періоду.</summary>
    public required DateOnly BeginDate { get; init; }

    /// <summary>Дата кінця періоду.</summary>
    public required DateOnly EndDate { get; init; }

    /// <summary>Формат тексту розкладу: united або separated.</summary>
    public ScheduleTextFormat TextFormat { get; init; } = ScheduleTextFormat.Separated;

    /// <summary>Показувати порожні дні (<c>show_empty=yes</c>).</summary>
    public bool ShowEmptyDays { get; init; }

    /// <summary>
    /// Показувати повний склад потоку (лише для <see cref="ScheduleTextFormat.Separated"/>).
    /// </summary>
    public bool IncludeAllStreamComponents { get; init; }

    /// <summary>Перевизначення кодування для цього запиту.</summary>
    public TextEncodingMode? Encoding { get; init; }
}
