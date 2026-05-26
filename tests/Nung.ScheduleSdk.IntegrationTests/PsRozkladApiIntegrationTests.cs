using Nung.ScheduleSdk.Enums;
using Nung.ScheduleSdk.Exceptions;
using Nung.ScheduleSdk.Models;
using Nung.ScheduleSdk.Requests;

namespace Nung.ScheduleSdk.IntegrationTests;

/// <summary>
/// Інтеграційні тести проти https://dekanat.nung.edu.ua/cgi-bin/timetable_export.cgi
/// </summary>
[Collection(IntegrationTestCollection.Name)]
[Trait("Category", "Integration")]
public sealed class PsRozkladApiIntegrationTests(PsRozkladClientFixture fixture)
{
    private readonly PsRozkladClient _client = fixture.Client;

    [Fact]
    public async Task GetObjectList_GroupsWithIds_ReturnsDepartments()
    {
        if (!IntegrationTestSkip.IsEnabled)
        {
            return;
        }

        PsRozkladExport export = await _client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Group,
            IncludeIds = true,
        });

        Assert.NotNull(export.Departments);
        Assert.NotEmpty(export.Departments);
        Department firstDepartment = export.Departments[0];
        Assert.False(string.IsNullOrWhiteSpace(firstDepartment.Name));
        Assert.NotNull(firstDepartment.Objects);
        Assert.Contains(firstDepartment.Objects, o => !string.IsNullOrWhiteSpace(o.Id));
    }

    [Fact]
    public async Task GetRoomTypes_ReturnsKnownTypes()
    {
        if (!IntegrationTestSkip.IsEnabled)
        {
            return;
        }

        PsRozkladExport export = await _client.GetRoomTypesAsync(new RoomTypeListRequest());

        Assert.NotNull(export.RoomTypes);
        Assert.Contains(export.RoomTypes, t => t.FullName == "Лекційна");
    }

    [Fact]
    public async Task GetSchedule_ByGroupId_ReturnsItemsOrEmptySuccess()
    {
        if (!IntegrationTestSkip.IsEnabled)
        {
            return;
        }

        PsRozkladExport list = await _client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Group,
            IncludeIds = true,
        });

        string? groupId = list.Departments?
            .SelectMany(d => d.Objects ?? Array.Empty<ScheduleEntity>())
            .Select(o => o.Id)
            .FirstOrDefault(id => !string.IsNullOrWhiteSpace(id));

        Assert.False(string.IsNullOrWhiteSpace(groupId));

        PsRozkladExport schedule = await _client.GetScheduleAsync(new ScheduleRequest
        {
            Mode = RequestMode.Group,
            ObjectId = groupId,
            BeginDate = new DateOnly(2026, 2, 1),
            EndDate = new DateOnly(2026, 2, 7),
            TextFormat = ScheduleTextFormat.Separated,
        });

        Assert.NotNull(schedule.ScheduleItems);
        Assert.True(schedule.IsSuccess);
    }

    [Fact]
    public async Task GetSchedule_InvalidObjectId_ThrowsApiException()
    {
        if (!IntegrationTestSkip.IsEnabled)
        {
            return;
        }

        PsRozkladApiException exception = await Assert.ThrowsAsync<PsRozkladApiException>(() =>
            _client.GetScheduleAsync(new ScheduleRequest
            {
                Mode = RequestMode.Teacher,
                ObjectId = "999999999",
                BeginDate = new DateOnly(2026, 1, 1),
                EndDate = new DateOnly(2026, 1, 2),
            }));

        Assert.Equal(-90, exception.ErrorCode);
    }

    [Fact]
    public async Task GetObjectList_Windows1251_ReturnsReadableCyrillic()
    {
        if (!IntegrationTestSkip.IsEnabled)
        {
            return;
        }

        PsRozkladExport export = await _client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Group,
            IncludeIds = false,
            Encoding = TextEncodingMode.Windows1251,
        });

        string? name = export.Departments?[0].Objects?[0].Name;
        Assert.False(string.IsNullOrWhiteSpace(name));
        Assert.DoesNotContain('?', name);
    }
}
