using Dekanat.ScheduleSdk.Enums;

namespace Dekanat.ScheduleSdk.Options;

/// <summary>
/// Налаштування HTTP-клієнта для API експорту розкладу НУГ.
/// </summary>
public sealed class PsRozkladClientOptions
{
    /// <summary>
    /// Базова URL-адреса CGI-скрипта експорту за замовчуванням.
    /// </summary>
    public const string DefaultBaseUrl = "https://dekanat.nung.edu.ua/cgi-bin/timetable_export.cgi";

    /// <summary>
    /// Повна URL-адреса ендпоінта <c>timetable_export.cgi</c>.
    /// </summary>
    public Uri BaseUrl { get; set; } = new(DefaultBaseUrl);

    /// <summary>
    /// Кодування тексту у відповіді (<c>coding_mode</c>). За замовчуванням UTF-8.
    /// </summary>
    public TextEncodingMode Encoding { get; set; } = TextEncodingMode.Utf8;

    /// <summary>
    /// Таймаут HTTP-запиту. За замовчуванням 60 секунд.
    /// </summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Якщо <see langword="true"/>, при <c>code != 0</c> клієнт кидає <see cref="Exceptions.PsRozkladApiException"/>.
    /// </summary>
    public bool ThrowOnApiError { get; set; } = true;
}
