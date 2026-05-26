using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.IntegrationTests.Helpers;
using Dekanat.ScheduleSdk.Models;
using Dekanat.ScheduleSdk.Requests;

namespace Dekanat.ScheduleSdk.IntegrationTests.Endpoints;

/// <summary>
/// <c>req_type=room_type_list</c>.
/// </summary>
[Collection(IntegrationTestCollection.Name)]
[Trait("Category", "Integration")]
public sealed class RoomTypesEndpointTests(PsRozkladClientFixture fixture)
{
    private readonly PsRozkladClient _client = fixture.Client;

    [Fact]
    public async Task GetRoomTypes_ReturnsStandardTypes()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        PsRozkladExport export = await _client.GetRoomTypesAsync(new RoomTypeListRequest());

        Assert.True(export.IsSuccess);
        Assert.NotNull(export.RoomTypes);
        Assert.Contains(export.RoomTypes, t => t.FullName == "Лекційна");
        Assert.Contains(export.RoomTypes, t => t.ShortName == "лек");
        Assert.All(export.RoomTypes, t =>
        {
            Assert.False(string.IsNullOrWhiteSpace(t.FullName));
            Assert.False(string.IsNullOrWhiteSpace(t.ShortName));
        });
    }

    [Fact]
    public async Task GetRoomTypes_Utf8Explicit_ReturnsSuccess()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        PsRozkladExport export = await _client.GetRoomTypesAsync(new RoomTypeListRequest
        {
            Encoding = TextEncodingMode.Utf8,
        });

        Assert.True(export.IsSuccess);
        Assert.NotEmpty(export.RoomTypes!);
    }
}
