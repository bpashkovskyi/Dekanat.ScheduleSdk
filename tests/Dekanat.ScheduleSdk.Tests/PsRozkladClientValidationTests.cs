using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.Requests;
using Dekanat.ScheduleSdk.Tests.Helpers;

namespace Dekanat.ScheduleSdk.Tests;

/// <summary>
/// Валідація параметрів на клієнті до відправки HTTP-запиту.
/// </summary>
[Trait("Category", "Unit")]
public sealed class PsRozkladClientValidationTests
{
    [Fact]
    public async Task GetObjectListAsync_NullRequest_ThrowsArgumentNullException()
    {
        (PsRozkladClient client, _) = PsRozkladClientTestFactory.Create();

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.GetObjectListAsync(null!));
    }

    [Fact]
    public async Task GetScheduleAsync_NoObjectSelector_ThrowsArgumentException()
    {
        (PsRozkladClient client, _) = PsRozkladClientTestFactory.Create();

        ScheduleRequest request = new()
        {
            Mode = RequestMode.Group,
            BeginDate = new DateOnly(2026, 1, 1),
            EndDate = new DateOnly(2026, 1, 31),
        };

        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            client.GetScheduleAsync(request));

        Assert.Equal("request", exception.ParamName);
    }

    [Fact]
    public async Task GetScheduleAsync_MultipleObjectSelectors_ThrowsArgumentException()
    {
        (PsRozkladClient client, _) = PsRozkladClientTestFactory.Create();

        ScheduleRequest request = new()
        {
            Mode = RequestMode.Group,
            ObjectId = "-1",
            ObjectName = "АТ-22-1",
            BeginDate = new DateOnly(2026, 1, 1),
            EndDate = new DateOnly(2026, 1, 31),
        };

        await Assert.ThrowsAsync<ArgumentException>(() => client.GetScheduleAsync(request));
    }

    [Fact]
    public async Task GetScheduleAsync_EndBeforeBegin_ThrowsArgumentException()
    {
        (PsRozkladClient client, _) = PsRozkladClientTestFactory.Create();

        ScheduleRequest request = new()
        {
            Mode = RequestMode.Group,
            ObjectId = "-1",
            BeginDate = new DateOnly(2026, 2, 10),
            EndDate = new DateOnly(2026, 2, 1),
        };

        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            client.GetScheduleAsync(request));

        Assert.Contains("EndDate", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetFreeRoomsAsync_NullRequest_ThrowsArgumentNullException()
    {
        (PsRozkladClient client, _) = PsRozkladClientTestFactory.Create();

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetFreeRoomsAsync(null!));
    }

    [Fact]
    public async Task GetRoomTypesAsync_NullRequest_ThrowsArgumentNullException()
    {
        (PsRozkladClient client, _) = PsRozkladClientTestFactory.Create();

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetRoomTypesAsync(null!));
    }

    [Fact]
    public async Task SendAsync_NullQuery_ThrowsArgumentNullException()
    {
        (PsRozkladClient client, _) = PsRozkladClientTestFactory.Create();

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.SendAsync(null!));
    }
}
