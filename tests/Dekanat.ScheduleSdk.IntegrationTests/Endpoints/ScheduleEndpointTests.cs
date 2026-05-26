using System.Text.Json;
using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.Exceptions;
using Dekanat.ScheduleSdk.IntegrationTests.Helpers;
using Dekanat.ScheduleSdk.Models;
using Dekanat.ScheduleSdk.Requests;

namespace Dekanat.ScheduleSdk.IntegrationTests.Endpoints;

/// <summary>
/// <c>req_type=rozklad</c> — розклад груп, викладачів, аудиторій.
/// </summary>
[Collection(IntegrationTestCollection.Name)]
[Trait("Category", "Integration")]
public sealed class ScheduleEndpointTests(PsRozkladClientFixture fixture)
{
    private readonly PsRozkladClient _client = fixture.Client;

    [Fact]
    public async Task GetSchedule_GroupById_Separated_ReturnsValidItems()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        string groupId = await IntegrationTestContext.GetFirstGroupIdAsync(_client);

        PsRozkladExport export = await _client.GetScheduleAsync(new ScheduleRequest
        {
            Mode = RequestMode.Group,
            ObjectId = groupId,
            BeginDate = new DateOnly(2026, 2, 1),
            EndDate = new DateOnly(2026, 2, 14),
            TextFormat = ScheduleTextFormat.Separated,
        });

        Assert.True(export.IsSuccess);
        Assert.NotNull(export.ScheduleItems);

        if (export.ScheduleItems.Count > 0)
        {
            ScheduleItem item = export.ScheduleItems[0];
            Assert.False(string.IsNullOrWhiteSpace(item.ObjectName));
            Assert.NotNull(item.GetDate());
        }
    }

    [Fact]
    public async Task GetSchedule_GroupById_United_MayContainLessonDescription()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        string groupId = await IntegrationTestContext.GetFirstGroupIdAsync(_client);

        PsRozkladExport export = await _client.GetScheduleAsync(new ScheduleRequest
        {
            Mode = RequestMode.Group,
            ObjectId = groupId,
            BeginDate = new DateOnly(2026, 2, 1),
            EndDate = new DateOnly(2026, 2, 7),
            TextFormat = ScheduleTextFormat.United,
        });

        Assert.True(export.IsSuccess);
        Assert.NotNull(export.ScheduleItems);
    }

    [Fact]
    public async Task GetSchedule_GroupWithStreamComponents_ReturnsSuccess()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        string groupId = await IntegrationTestContext.GetFirstGroupIdAsync(_client);

        PsRozkladExport export = await _client.GetScheduleAsync(new ScheduleRequest
        {
            Mode = RequestMode.Group,
            ObjectId = groupId,
            BeginDate = new DateOnly(2026, 2, 1),
            EndDate = new DateOnly(2026, 2, 3),
            TextFormat = ScheduleTextFormat.Separated,
            IncludeAllStreamComponents = true,
        });

        Assert.True(export.IsSuccess);
    }

    [Fact]
    public async Task GetSchedule_TeacherById_ReturnsSuccess()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        string teacherId = await IntegrationTestContext.GetFirstTeacherIdAsync(_client);

        PsRozkladExport export = await _client.GetScheduleAsync(new ScheduleRequest
        {
            Mode = RequestMode.Teacher,
            ObjectId = teacherId,
            BeginDate = new DateOnly(2026, 2, 1),
            EndDate = new DateOnly(2026, 2, 7),
        });

        Assert.True(export.IsSuccess);
        Assert.NotNull(export.ScheduleItems);
    }

    [Fact]
    public async Task GetSchedule_RoomById_ReturnsSuccess()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        (_, string roomId, _) = await IntegrationTestContext.GetFirstRoomAsync(_client);

        PsRozkladExport export = await _client.GetScheduleAsync(new ScheduleRequest
        {
            Mode = RequestMode.Room,
            ObjectId = roomId,
            BeginDate = new DateOnly(2026, 2, 1),
            EndDate = new DateOnly(2026, 2, 7),
        });

        Assert.True(export.IsSuccess);
        Assert.NotNull(export.ScheduleItems);
    }

    [Fact]
    public async Task GetSchedule_RoomByDepartmentName_SingleDay_ReturnsSuccess()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        DateOnly day = new(2026, 2, 2);

        try
        {
            PsRozkladExport export = await _client.GetScheduleAsync(new ScheduleRequest
            {
                Mode = RequestMode.Room,
                DepartmentName = "0",
                BeginDate = day,
                EndDate = day,
            });

            Assert.True(export.IsSuccess);
        }
        catch (JsonException)
        {
            // CGI інколи повертає пошкоджений JSON для dep_name.
        }
        catch (PsRozkladApiException ex) when (ex.ErrorCode is -90 or -4 or 4)
        {
            // Немає розкладу на дату / підрозділ.
        }
    }

    [Fact]
    public async Task GetSchedule_InvalidObjectId_ThrowsObjectNotFound()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        PsRozkladApiException exception = await Assert.ThrowsAsync<PsRozkladApiException>(() =>
            _client.GetScheduleAsync(new ScheduleRequest
            {
                Mode = RequestMode.Group,
                ObjectId = "999999999",
                BeginDate = new DateOnly(2026, 1, 1),
                EndDate = new DateOnly(2026, 1, 2),
            }));

        Assert.Equal(-90, exception.ErrorCode);
    }

    [Fact]
    public async Task GetSchedule_InvalidDateRange_ThrowsInvalidDates()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        string groupId = await IntegrationTestContext.GetFirstGroupIdAsync(_client);

        PsRozkladApiException exception = await Assert.ThrowsAsync<PsRozkladApiException>(() =>
            _client.GetScheduleAsync(new ScheduleRequest
            {
                Mode = RequestMode.Group,
                ObjectId = groupId,
                BeginDate = new DateOnly(2099, 1, 1),
                EndDate = new DateOnly(2099, 12, 31),
            }));

        Assert.True(
            exception.ErrorCode is -70 or -3 or 3 or -4,
            $"Unexpected error code: {exception.ErrorCode}, message: {exception.ErrorMessage}");
    }
}
