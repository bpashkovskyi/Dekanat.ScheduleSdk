using System.Text.Json;
using Dekanat.ScheduleSdk.Json;
using Dekanat.ScheduleSdk.Models;

namespace Dekanat.ScheduleSdk.IntegrationTests;

/// <summary>
/// Перевірка десеріалізації на зразках реальних відповідей API (без мережі).
/// </summary>
public sealed class JsonDeserializationTests
{
    [Fact]
    public void Deserialize_SeparatedSchedule_MapsAllExpectedFields()
    {
        string json = """
            {"psrozklad_export":{"roz_items":[{"object":"НЗФм-25-1","date":"11.09.2025","comment":"1","lesson_number":"1","lesson_name":"1","lesson_time":"08:00-09:20","half":"","teacher":"доцент Федорів В.В.","teachers_add":"","room":"5.101.ауд.","group":"","title":"Теорія","type":"Л","replacement":"","reservation":"","online":"Так","comment4link":"","link":""}],"code":"0"}}
            """;

        PsRozkladResponse? response = JsonSerializer.Deserialize<PsRozkladResponse>(
            json,
            PsRozkladJsonSerializerOptions.Default);

        Assert.NotNull(response?.Export);
        Assert.Equal(0, response.Export.Code);
        ScheduleItem item = Assert.Single(response.Export.ScheduleItems!);
        Assert.Equal("НЗФм-25-1", item.ObjectName);
        Assert.Equal("доцент Федорів В.В.", item.Teacher);
        Assert.Equal("5.101.ауд.", item.Room);
        Assert.Equal(new DateOnly(2025, 9, 11), item.GetDate());
    }

    [Fact]
    public void Deserialize_ApiError_ParsesNegativeErrorCode()
    {
        string json = """
            {"psrozklad_export":{"error":{"error_message":"Об`єкт не знайдено","errorcode":"-90"},"code":"90"}}
            """;

        PsRozkladResponse? response = JsonSerializer.Deserialize<PsRozkladResponse>(
            json,
            PsRozkladJsonSerializerOptions.Default);

        Assert.NotNull(response?.Export);
        Assert.False(response.Export.IsSuccess);
        Assert.Equal(-90, response.Export.GetNormalizedErrorCode());
        Assert.Equal("Об`єкт не знайдено", response.Export.Error?.Message);
    }

    [Fact]
    public void Deserialize_RoomTypes_ReturnsObjectsArray()
    {
        string json = """
            {"psrozklad_export":{"objects":[{"full":"Лекційна","short":"лек"}],"code":"0"}}
            """;

        PsRozkladResponse? response = JsonSerializer.Deserialize<PsRozkladResponse>(
            json,
            PsRozkladJsonSerializerOptions.Default);

        RoomType roomType = Assert.Single(response!.Export!.RoomTypes!);
        Assert.Equal("Лекційна", roomType.FullName);
        Assert.Equal("лек", roomType.ShortName);
    }
}
