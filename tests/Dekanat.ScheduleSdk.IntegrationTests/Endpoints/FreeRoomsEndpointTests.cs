using System.Text.Json;

using Dekanat.ScheduleSdk.Exceptions;
using Dekanat.ScheduleSdk.IntegrationTests.Helpers;
using Dekanat.ScheduleSdk.Models;
using Dekanat.ScheduleSdk.Requests;

namespace Dekanat.ScheduleSdk.IntegrationTests.Endpoints;

/// <summary>
/// <c>req_type=free_rooms_list</c> — вільні аудиторії.
/// </summary>
/// <remarks>
/// Ендпоінт на сервері інколи повертає некоректний JSON; тести фіксують очікувану поведінку клієнта.
/// </remarks>
[Collection(IntegrationTestCollection.Name)]
[Trait("Category", "Integration")]
public sealed class FreeRoomsEndpointTests(PsRozkladClientFixture fixture)
{
    private readonly PsRozkladClient _client = fixture.Client;

    [Fact]
    public async Task GetFreeRooms_MinimalParameters_ReturnsResponseOrApiError()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        try
        {
            PsRozkladExport export = await _client.GetFreeRoomsAsync(new FreeRoomsListRequest
            {
                Date = new DateOnly(2026, 2, 2),
                LessonNumber = 1,
            });

            Assert.True(export.IsSuccess || export.Error is not null);
        }
        catch (PsRozkladApiException ex) when (ex.ErrorCode is -4 or 4 or -100 or -60)
        {
            // Немає вільних аудиторій або помилка параметрів на CGI.
        }
        catch (JsonException)
        {
            // Відома нестабільність відповіді free_rooms_list на стороні CGI.
        }
    }

    [Fact]
    public async Task GetFreeRooms_WithBuildingFilter_ReturnsResponseOrApiError()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        (string buildingName, _, _) = await IntegrationTestContext.GetFirstRoomAsync(_client);

        try
        {
            PsRozkladExport export = await _client.GetFreeRoomsAsync(new FreeRoomsListRequest
            {
                Date = new DateOnly(2026, 2, 2),
                LessonNumber = 2,
                BuildingName = buildingName,
            });

            if (export.IsSuccess && export.FreeRooms is not null)
            {
                Assert.All(export.FreeRooms, entry =>
                {
                    Assert.False(string.IsNullOrWhiteSpace(entry.Date));
                    Assert.False(string.IsNullOrWhiteSpace(entry.Lesson));
                });
            }
        }
        catch (PsRozkladApiException ex)
        {
            Assert.True(ex.ErrorCode is -4 or -100 or -60, $"Code: {ex.ErrorCode}");
        }
        catch (JsonException)
        {
            // Див. коментар у першому тесті.
        }
    }

    [Fact]
    public async Task GetFreeRooms_WithCapacityAndRoomType_ReturnsResponseOrApiError()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        PsRozkladExport types = await _client.GetRoomTypesAsync(new RoomTypeListRequest());
        string? shortType = types.RoomTypes?.FirstOrDefault()?.ShortName;

        try
        {
            await _client.GetFreeRoomsAsync(new FreeRoomsListRequest
            {
                Date = new DateOnly(2026, 2, 2),
                LessonNumber = 1,
                BuildingName = "0",
                RoomType = shortType,
                MinimumCapacity = 20,
                MaximumCapacity = 200,
            });
        }
        catch (PsRozkladApiException)
        {
            // Прийнятно для живого API.
        }
        catch (JsonException)
        {
            // Прийнятно для живого API.
        }
    }
}
