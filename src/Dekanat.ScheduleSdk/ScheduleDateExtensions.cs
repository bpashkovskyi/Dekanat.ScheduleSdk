using System.Globalization;
using Dekanat.ScheduleSdk.Models;

namespace Dekanat.ScheduleSdk;

/// <summary>
/// Допоміжні методи для роботи з датами у форматі API (<c>dd.MM.yyyy</c>).
/// </summary>
public static class ScheduleDateExtensions
{
    private const string ApiDateFormat = "dd.MM.yyyy";

    /// <summary>
    /// Розбирає рядок дати з API у <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="apiDate">Дата у форматі <c>dd.MM.yyyy</c>.</param>
    /// <returns>Розпарсена дата або <see langword="null"/>, якщо рядок порожній.</returns>
    /// <exception cref="FormatException">Якщо формат дати некоректний.</exception>
    public static DateOnly? ParseApiDate(this string? apiDate)
    {
        if (string.IsNullOrWhiteSpace(apiDate))
        {
            return null;
        }

        return DateOnly.ParseExact(apiDate, ApiDateFormat, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Повертає дату елемента розкладу як <see cref="DateOnly"/>.
    /// </summary>
    public static DateOnly? GetDate(this ScheduleItem item) =>
        item.Date.ParseApiDate();
}
