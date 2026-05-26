using Nung.ScheduleSdk.Models;

namespace Nung.ScheduleSdk.Exceptions;

/// <summary>
/// Виняток, що виникає коли API ПС-Розклад повернув код помилки або об'єкт <c>error</c>.
/// </summary>
public sealed class PsRozkladApiException : Exception
{
    /// <summary>
    /// Створює виняток на основі тіла відповіді <see cref="PsRozkladExport"/>.
    /// </summary>
    /// <param name="export">Розпарсена відповідь API.</param>
    public PsRozkladApiException(PsRozkladExport export)
        : base(BuildMessage(export))
    {
        Export = export;
        ErrorCode = export.GetNormalizedErrorCode();
        ErrorMessage = export.Error?.Message;
    }

    /// <summary>Повна відповідь API на момент помилки.</summary>
    public PsRozkladExport Export { get; }

    /// <summary>Нормалізований числовий код помилки.</summary>
    public int? ErrorCode { get; }

    /// <summary>Текст помилки з поля <c>error_message</c>.</summary>
    public string? ErrorMessage { get; }

    private static string BuildMessage(PsRozkladExport export)
    {
        int? code = export.GetNormalizedErrorCode();
        string message = export.Error?.Message ?? "Невідома помилка API ПС-Розклад.";
        return code is null
            ? message
            : $"Помилка API ПС-Розклад (код {code}): {message}";
    }
}
