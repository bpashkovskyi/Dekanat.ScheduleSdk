using System.Text.Json;
using Dekanat.ScheduleSdk.Json;
using Dekanat.ScheduleSdk.Models;

namespace Dekanat.ScheduleSdk.Tests;

/// <summary>
/// Десеріалізація всіх відомих фрагментів JSON-відповіді API.
/// </summary>
[Trait("Category", "Unit")]
public sealed class JsonDeserializationTests
{
    [Fact]
    public void Deserialize_ObjectListGroup_MapsDepartmentsAndIds()
    {
        string json = """
            {"psrozklad_export":{"departments":[{"name":"Факультет","objects":[{"name":"АТ-22-1","ID":"-1664"}]}],"code":"0"}}
            """;

        PsRozkladExport export = Deserialize(json);

        Department department = Assert.Single(export.Departments!);
        Assert.Equal("Факультет", department.Name);
        ScheduleEntity entity = Assert.Single(department.Objects!);
        Assert.Equal("АТ-22-1", entity.Name);
        Assert.Equal("-1664", entity.Id);
    }

    [Fact]
    public void Deserialize_ObjectListTeacher_MapsPatronymicFields()
    {
        string json = """
            {"psrozklad_export":{"departments":[{"name":"Кафедра","objects":[{"name":"Іванов І.І.","P":"Іванов","I":"Іван","B":"Іванович"}]}],"code":"0"}}
            """;

        ScheduleEntity teacher = Assert.Single(Deserialize(json).Departments![0].Objects!);
        Assert.Equal("Іванов", teacher.LastName);
        Assert.Equal("Іван", teacher.FirstName);
        Assert.Equal("Іванович", teacher.Patronymic);
    }

    [Fact]
    public void Deserialize_ObjectListRoom_MapsBlocks()
    {
        string json = """
            {"psrozklad_export":{"blocks":[{"name":"5","objects":[{"name":"5.101.ауд.","ID":"42"}]}],"code":"0"}}
            """;

        Building block = Assert.Single(Deserialize(json).Blocks!);
        Assert.Equal("5", block.Name);
        Assert.Equal("42", block.Objects![0].Id);
    }

    [Fact]
    public void Deserialize_SeparatedSchedule_MapsAllColumns()
    {
        string json = """
            {"psrozklad_export":{"roz_items":[{"object":"НЗФм-25-1","date":"11.09.2025","comment":"1","lesson_number":"1","lesson_name":"1","lesson_time":"08:00-09:20","half":"","teacher":"доцент Федорів В.В.","teachers_add":"доп.","room":"5.101.ауд.","group":"потік","title":"Теорія","type":"Л","replacement":"заміна","reservation":"резерв","online":"Так","comment4link":"комент","link":"https://x"}],"code":"0"}}
            """;

        ScheduleItem item = Assert.Single(Deserialize(json).ScheduleItems!);
        Assert.Equal("доцент Федорів В.В.", item.Teacher);
        Assert.Equal("доп.", item.AdditionalTeachers);
        Assert.Equal("5.101.ауд.", item.Room);
        Assert.Equal("потік", item.Group);
        Assert.Equal("Теорія", item.Title);
        Assert.Equal("Л", item.Type);
        Assert.Equal("заміна", item.Replacement);
        Assert.Equal("резерв", item.Reservation);
        Assert.Equal("Так", item.Online);
        Assert.Equal("комент", item.LinkComment);
        Assert.Equal("https://x", item.Link);
        Assert.Equal(new DateOnly(2025, 9, 11), item.GetDate());
    }

    [Fact]
    public void Deserialize_UnitedSchedule_MapsLessonDescription()
    {
        string json = """
            {"psrozklad_export":{"roz_items":[{"object":"АТ-22-1","date":"02.02.2026","lesson_number":"1","lesson_description":"Дисципліна викладач ауд."}],"code":"0"}}
            """;

        ScheduleItem item = Assert.Single(Deserialize(json).ScheduleItems!);
        Assert.Equal("Дисципліна викладач ауд.", item.LessonDescription);
    }

    [Fact]
    public void Deserialize_RoomTypes_MapsFullAndShort()
    {
        string json = """
            {"psrozklad_export":{"objects":[{"full":"Лекційна","short":"лек"}],"code":"0"}}
            """;

        RoomType type = Assert.Single(Deserialize(json).RoomTypes!);
        Assert.Equal("Лекційна", type.FullName);
        Assert.Equal("лек", type.ShortName);
    }

    [Fact]
    public void Deserialize_FreeRooms_MapsEntries()
    {
        string json = """
            {"psrozklad_export":{"free_rooms":[{"date":"02.02.2026","lesson":"1","rooms":["5.101.ауд.","5.102.ауд."]}],"code":"0"}}
            """;

        FreeRoomsEntry entry = Assert.Single(Deserialize(json).FreeRooms!);
        Assert.Equal("02.02.2026", entry.Date);
        Assert.Equal("1", entry.Lesson);
        Assert.Equal(2, entry.Rooms!.Count);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("-90", -90)]
    [InlineData("90", 90)]
    public void Deserialize_CodeAsString_ParsesInteger(string code, int expected)
    {
        string json = "{\"psrozklad_export\":{\"code\":\"" + code + "\"}}";
        Assert.Equal(expected, Deserialize(json).Code);
    }

    [Fact]
    public void Deserialize_ApiError_NormalizesViaExport()
    {
        string json = """
            {"psrozklad_export":{"error":{"error_message":"Об`єкт не знайдено","errorcode":"-90"},"code":"90"}}
            """;

        PsRozkladExport export = Deserialize(json);
        Assert.False(export.IsSuccess);
        Assert.Equal(-90, export.GetNormalizedErrorCode());
        Assert.Equal("Об`єкт не знайдено", export.Error?.Message);
    }

    private static PsRozkladExport Deserialize(string json)
    {
        PsRozkladResponse? response = JsonSerializer.Deserialize<PsRozkladResponse>(
            json,
            PsRozkladJsonSerializerOptions.Default);

        Assert.NotNull(response?.Export);
        return response.Export;
    }
}
