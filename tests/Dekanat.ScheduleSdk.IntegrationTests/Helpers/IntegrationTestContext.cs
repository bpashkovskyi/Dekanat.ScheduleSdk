using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.Models;
using Dekanat.ScheduleSdk.Requests;

namespace Dekanat.ScheduleSdk.IntegrationTests.Helpers;

/// <summary>
/// Спільні методи для інтеграційних тестів (отримання ID з живого API).
/// </summary>
internal static class IntegrationTestContext
{
    public static bool IsEnabled => IntegrationTestSkip.IsEnabled;

    public static async Task<string> GetFirstGroupIdAsync(PsRozkladClient client)
    {
        PsRozkladExport list = await client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Group,
            IncludeIds = true,
        });

        string? id = list.Departments?
            .SelectMany(d => d.Objects ?? Array.Empty<ScheduleEntity>())
            .Select(o => o.Id)
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

        Assert.NotNull(id);
        return id;
    }

    public static async Task<string> GetFirstTeacherIdAsync(PsRozkladClient client)
    {
        PsRozkladExport list = await client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Teacher,
            IncludeIds = true,
        });

        string? id = list.Departments?
            .SelectMany(d => d.Objects ?? Array.Empty<ScheduleEntity>())
            .Select(o => o.Id)
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

        Assert.NotNull(id);
        return id;
    }

    public static async Task<(string BuildingName, string RoomId, string RoomName)> GetFirstRoomAsync(
        PsRozkladClient client)
    {
        PsRozkladExport list = await client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Room,
            IncludeIds = true,
        });

        Building? block = list.Blocks?
            .Where(b => b.Objects?.Count > 0 && b.Name is not "." and not "..")
            .OrderByDescending(b => b.Objects!.Count)
            .FirstOrDefault();

        Assert.NotNull(block);
        ScheduleEntity room = block!.Objects!.First(o => !string.IsNullOrWhiteSpace(o.Id));
        Assert.NotNull(room.Id);
        Assert.NotNull(room.Name);
        return (block.Name!, room.Id, room.Name);
    }

    public static async Task<string> GetFirstGroupNameAsync(PsRozkladClient client)
    {
        PsRozkladExport list = await client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Group,
            IncludeIds = false,
        });

        string? name = list.Departments?
            .SelectMany(d => d.Objects ?? Array.Empty<ScheduleEntity>())
            .Select(o => o.Name)
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

        Assert.NotNull(name);
        return name;
    }
}
