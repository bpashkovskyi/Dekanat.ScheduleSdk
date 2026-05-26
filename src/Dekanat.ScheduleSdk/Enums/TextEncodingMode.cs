namespace Dekanat.ScheduleSdk.Enums;

/// <summary>
/// Значення параметра <c>coding_mode</c> — кодування тексту у відповіді API.
/// </summary>
/// <remarks>
/// Для JSON рекомендовано <see cref="Utf8"/>. <see cref="Windows1251"/> залишено для сумісності з legacy-клієнтами.
/// </remarks>
public enum TextEncodingMode
{
    /// <summary>Кодування UTF-8 (<c>coding_mode=UTF8</c>).</summary>
    Utf8,

    /// <summary>Кодування Windows-1251 (<c>coding_mode=WINDOWS-1251</c>).</summary>
    Windows1251,
}
