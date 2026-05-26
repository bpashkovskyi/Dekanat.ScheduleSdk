using Nung.ScheduleSdk.Enums;

namespace Nung.ScheduleSdk.Requests;

/// <summary>
/// Параметри запиту переліку об'єктів (<c>req_type=obj_list</c>).
/// </summary>
public sealed class ObjectListRequest
{
    /// <summary>Режим: групи, викладачі або аудиторії.</summary>
    public required RequestMode Mode { get; init; }

    /// <summary>
    /// Якщо <see langword="true"/>, API додасть поле <c>ID</c> до кожного об'єкта (<c>show_ID=yes</c>).
    /// </summary>
    public bool IncludeIds { get; init; }

    /// <summary>
    /// Перевизначення кодування для цього запиту. Якщо <see langword="null"/> — використовується значення з опцій клієнта.
    /// </summary>
    public TextEncodingMode? Encoding { get; init; }
}
