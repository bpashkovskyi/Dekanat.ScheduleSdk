using Dekanat.ScheduleSdk.Models;

namespace Dekanat.ScheduleSdk.Tests;

/// <summary>
/// Парсинг дат формату API <c>dd.MM.yyyy</c>.
/// </summary>
[Trait("Category", "Unit")]
public sealed class ScheduleDateExtensionsTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ParseApiDate_Empty_ReturnsNull(string? input)
    {
        Assert.Null(input.ParseApiDate());
    }

    [Fact]
    public void ParseApiDate_Valid_ReturnsDateOnly()
    {
        DateOnly? date = "26.05.2026".ParseApiDate();
        Assert.Equal(new DateOnly(2026, 5, 26), date);
    }

    [Fact]
    public void ParseApiDate_Invalid_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => "2026-05-26".ParseApiDate());
    }

    [Fact]
    public void GetDate_OnScheduleItem_ReturnsParsedDate()
    {
        ScheduleItem item = new() { Date = "02.02.2026" };
        Assert.Equal(new DateOnly(2026, 2, 2), item.GetDate());
    }
}
