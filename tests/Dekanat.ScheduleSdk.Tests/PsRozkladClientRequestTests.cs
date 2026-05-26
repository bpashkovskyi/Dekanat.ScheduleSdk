using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.Exceptions;
using Dekanat.ScheduleSdk.Options;
using Dekanat.ScheduleSdk.Requests;
using Dekanat.ScheduleSdk.Tests.Helpers;

namespace Dekanat.ScheduleSdk.Tests;

/// <summary>
/// Перевірка формування HTTP-запитів для кожного публічного методу клієнта.
/// </summary>
[Trait("Category", "Unit")]
public sealed class PsRozkladClientRequestTests
{
    [Fact]
    public async Task GetObjectListAsync_GroupWithIds_BuildsExpectedQuery()
    {
        (PsRozkladClient client, RecordingHttpMessageHandler handler) = PsRozkladClientTestFactory.Create();

        await client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Group,
            IncludeIds = true,
            Encoding = TextEncodingMode.Utf8,
        });

        IReadOnlyDictionary<string, string> query = handler.GetLastQuery();
        Assert.Equal("group", query["req_mode"]);
        Assert.Equal("obj_list", query["req_type"]);
        Assert.Equal("json", query["req_format"]);
        Assert.Equal("UTF8", query["coding_mode"]);
        Assert.Equal("yes", query["show_ID"]);
    }

    [Fact]
    public async Task GetObjectListAsync_TeacherWithoutIds_OmitsShowId()
    {
        (PsRozkladClient client, RecordingHttpMessageHandler handler) = PsRozkladClientTestFactory.Create();

        await client.GetObjectListAsync(new ObjectListRequest { Mode = RequestMode.Teacher });

        IReadOnlyDictionary<string, string> query = handler.GetLastQuery();
        Assert.Equal("teacher", query["req_mode"]);
        Assert.False(query.ContainsKey("show_ID"));
    }

    [Fact]
    public async Task GetObjectListAsync_Room_BuildsRoomObjList()
    {
        (PsRozkladClient client, RecordingHttpMessageHandler handler) = PsRozkladClientTestFactory.Create();

        await client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Room,
            IncludeIds = true,
        });

        IReadOnlyDictionary<string, string> query = handler.GetLastQuery();
        Assert.Equal("room", query["req_mode"]);
        Assert.Equal("obj_list", query["req_type"]);
    }

    [Fact]
    public async Task GetScheduleAsync_ByObjectId_IncludesDatesAndSeparatedFormat()
    {
        (PsRozkladClient client, RecordingHttpMessageHandler handler) = PsRozkladClientTestFactory.Create();

        await client.GetScheduleAsync(new ScheduleRequest
        {
            Mode = RequestMode.Group,
            ObjectId = "-1664",
            BeginDate = new DateOnly(2026, 2, 1),
            EndDate = new DateOnly(2026, 2, 7),
            TextFormat = ScheduleTextFormat.Separated,
            ShowEmptyDays = true,
            IncludeAllStreamComponents = true,
        });

        IReadOnlyDictionary<string, string> query = handler.GetLastQuery();
        Assert.Equal("rozklad", query["req_type"]);
        Assert.Equal("-1664", query["OBJ_ID"]);
        Assert.Equal("01.02.2026", query["begin_date"]);
        Assert.Equal("07.02.2026", query["end_date"]);
        Assert.Equal("separated", query["ros_text"]);
        Assert.Equal("yes", query["show_empty"]);
        Assert.Equal("yes", query["all_stream_components"]);
    }

    [Fact]
    public async Task GetScheduleAsync_ByObjectName_UsesObjNameParameter()
    {
        (PsRozkladClient client, RecordingHttpMessageHandler handler) = PsRozkladClientTestFactory.Create();

        await client.GetScheduleAsync(new ScheduleRequest
        {
            Mode = RequestMode.Teacher,
            ObjectName = "Іванов І.І.",
            BeginDate = new DateOnly(2026, 3, 1),
            EndDate = new DateOnly(2026, 3, 1),
            TextFormat = ScheduleTextFormat.United,
        });

        IReadOnlyDictionary<string, string> query = handler.GetLastQuery();
        Assert.Equal("Іванов І.І.", query["OBJ_name"]);
        Assert.Equal("united", query["ros_text"]);
        Assert.False(query.ContainsKey("OBJ_ID"));
    }

    [Fact]
    public async Task GetScheduleAsync_ByDepartmentName_UsesDepNameParameter()
    {
        (PsRozkladClient client, RecordingHttpMessageHandler handler) = PsRozkladClientTestFactory.Create();

        await client.GetScheduleAsync(new ScheduleRequest
        {
            Mode = RequestMode.Room,
            DepartmentName = "0",
            BeginDate = new DateOnly(2026, 2, 2),
            EndDate = new DateOnly(2026, 2, 2),
        });

        IReadOnlyDictionary<string, string> query = handler.GetLastQuery();
        Assert.Equal("0", query["dep_name"]);
    }

    [Fact]
    public async Task GetFreeRoomsAsync_IncludesAllOptionalFilters()
    {
        (PsRozkladClient client, RecordingHttpMessageHandler handler) = PsRozkladClientTestFactory.Create();

        await client.GetFreeRoomsAsync(new FreeRoomsListRequest
        {
            Date = new DateOnly(2026, 2, 2),
            LessonNumber = 3,
            BuildingName = "5",
            RoomType = "лек",
            MinimumCapacity = 30,
            MaximumCapacity = 120,
        });

        IReadOnlyDictionary<string, string> query = handler.GetLastQuery();
        Assert.Equal("free_rooms_list", query["req_type"]);
        Assert.Equal("room", query["req_mode"]);
        Assert.Equal("02.02.2026", query["rooms_date"]);
        Assert.Equal("3", query["lesson"]);
        Assert.Equal("5", query["block_name"]);
        Assert.Equal("лек", query["room_type"]);
        Assert.Equal("30", query["size_min"]);
        Assert.Equal("120", query["size_max"]);
    }

    [Fact]
    public async Task GetRoomTypesAsync_BuildsRoomTypeListQuery()
    {
        (PsRozkladClient client, RecordingHttpMessageHandler handler) = PsRozkladClientTestFactory.Create();

        await client.GetRoomTypesAsync(new RoomTypeListRequest
        {
            Encoding = TextEncodingMode.Windows1251,
        });

        IReadOnlyDictionary<string, string> query = handler.GetLastQuery();
        Assert.Equal("room_type_list", query["req_type"]);
        Assert.Equal("WINDOWS-1251", query["coding_mode"]);
    }

    [Fact]
    public async Task SendAsync_WhenThrowOnApiErrorFalse_ReturnsErrorPayload()
    {
        string errorJson = """
            {"psrozklad_export":{"error":{"error_message":"test","errorcode":"-90"},"code":"90"}}
            """;

        (PsRozkladClient client, _) = PsRozkladClientTestFactory.Create(
            new PsRozkladClientOptions { ThrowOnApiError = false },
            errorJson);

        Models.PsRozkladResponse response = await client.SendAsync(new Dictionary<string, string>
        {
            ["req_mode"] = "group",
            ["req_type"] = "rozklad",
        });

        Assert.False(response.Export!.IsSuccess);
        Assert.Equal(-90, response.Export.GetNormalizedErrorCode());
    }

    [Fact]
    public async Task SendAsync_WhenThrowOnApiErrorTrue_ThrowsPsRozkladApiException()
    {
        string errorJson = """
            {"psrozklad_export":{"error":{"error_message":"not found","errorcode":"-4"},"code":"4"}}
            """;

        (PsRozkladClient client, _) = PsRozkladClientTestFactory.Create(responseJson: errorJson);

        PsRozkladApiException exception = await Assert.ThrowsAsync<PsRozkladApiException>(() =>
            client.SendAsync(new Dictionary<string, string> { ["req_type"] = "obj_list", ["req_mode"] = "group" }));

        Assert.Equal(-4, exception.ErrorCode);
    }

    [Fact]
    public async Task SendAsync_MissingExportRoot_ThrowsInvalidOperationException()
    {
        (PsRozkladClient client, RecordingHttpMessageHandler handler) = PsRozkladClientTestFactory.Create();
        handler.ResponseContent = """{"other":{}}""";

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            client.SendAsync(new Dictionary<string, string> { ["req_mode"] = "group", ["req_type"] = "obj_list" }));
    }
}
