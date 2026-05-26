using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.Internal;

namespace Dekanat.ScheduleSdk.Tests;

/// <summary>
/// Перевірка формування query-параметрів для CGI API.
/// </summary>
[Trait("Category", "Unit")]
public sealed class ApiQueryBuilderTests
{
    [Theory]
    [InlineData(TextEncodingMode.Utf8, "UTF8")]
    [InlineData(TextEncodingMode.Windows1251, "WINDOWS-1251")]
    public void CreateBaseQuery_ContainsJsonFormatAndEncoding(TextEncodingMode mode, string expectedCoding)
    {
        Dictionary<string, string> query = ApiQueryBuilder.CreateBaseQuery(mode);

        Assert.Equal("json", query["req_format"]);
        Assert.Equal(expectedCoding, query["coding_mode"]);
    }

    [Theory]
    [InlineData(RequestMode.Group, "group")]
    [InlineData(RequestMode.Teacher, "teacher")]
    [InlineData(RequestMode.Room, "room")]
    public void ToRequestMode_MapsAllValues(RequestMode mode, string expected)
    {
        Assert.Equal(expected, ApiQueryBuilder.ToRequestMode(mode));
    }

    [Theory]
    [InlineData(RequestType.ObjectList, "obj_list")]
    [InlineData(RequestType.Schedule, "rozklad")]
    [InlineData(RequestType.FreeRoomsList, "free_rooms_list")]
    [InlineData(RequestType.RoomTypeList, "room_type_list")]
    public void ToRequestType_MapsAllValues(RequestType type, string expected)
    {
        Assert.Equal(expected, ApiQueryBuilder.ToRequestType(type));
    }

    [Theory]
    [InlineData(ScheduleTextFormat.United, "united")]
    [InlineData(ScheduleTextFormat.Separated, "separated")]
    public void ToScheduleTextFormat_MapsAllValues(ScheduleTextFormat format, string expected)
    {
        Assert.Equal(expected, ApiQueryBuilder.ToScheduleTextFormat(format));
    }

    [Fact]
    public void FormatApiDate_UsesDdMmYyyy()
    {
        string formatted = ApiQueryBuilder.FormatApiDate(new DateOnly(2026, 2, 5));
        Assert.Equal("05.02.2026", formatted);
    }
}
