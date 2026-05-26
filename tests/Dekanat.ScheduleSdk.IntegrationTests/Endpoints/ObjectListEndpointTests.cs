using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.IntegrationTests.Helpers;
using Dekanat.ScheduleSdk.Models;
using Dekanat.ScheduleSdk.Requests;

namespace Dekanat.ScheduleSdk.IntegrationTests.Endpoints;

/// <summary>
/// <c>req_type=obj_list</c> для груп, викладачів і аудиторій.
/// </summary>
[Collection(IntegrationTestCollection.Name)]
[Trait("Category", "Integration")]
public sealed class ObjectListEndpointTests(PsRozkladClientFixture fixture)
{
    private readonly PsRozkladClient _client = fixture.Client;

    [Theory]
    [InlineData(RequestMode.Group, true)]
    [InlineData(RequestMode.Group, false)]
    [InlineData(RequestMode.Teacher, true)]
    [InlineData(RequestMode.Teacher, false)]
    [InlineData(RequestMode.Room, true)]
    [InlineData(RequestMode.Room, false)]
    public async Task GetObjectList_AllModes_ReturnsSuccess(RequestMode mode, bool includeIds)
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        PsRozkladExport export = await _client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = mode,
            IncludeIds = includeIds,
        });

        Assert.True(export.IsSuccess);
        Assert.Equal(0, export.Code);

        if (mode == RequestMode.Room)
        {
            Assert.NotNull(export.Blocks);
            Assert.NotEmpty(export.Blocks);
        }
        else
        {
            Assert.NotNull(export.Departments);
            Assert.NotEmpty(export.Departments);
        }

        if (includeIds)
        {
            IEnumerable<ScheduleEntity> objects = mode == RequestMode.Room
                ? export.Blocks!.SelectMany(b => b.Objects ?? [])
                : export.Departments!.SelectMany(d => d.Objects ?? []);

            Assert.Contains(objects, o => !string.IsNullOrWhiteSpace(o.Id));
        }
    }

    [Fact]
    public async Task GetObjectList_Teachers_ContainsPatronymicParts()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        PsRozkladExport export = await _client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Teacher,
            IncludeIds = true,
        });

        ScheduleEntity? teacher = export.Departments?
            .SelectMany(d => d.Objects ?? [])
            .FirstOrDefault(o =>
                !string.IsNullOrWhiteSpace(o.LastName) &&
                !string.IsNullOrWhiteSpace(o.FirstName));

        Assert.NotNull(teacher);
    }

    [Fact]
    public async Task GetObjectList_Utf8_DefaultEncoding_ReturnsCyrillic()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        PsRozkladExport export = await _client.GetObjectListAsync(new ObjectListRequest
        {
            Mode = RequestMode.Group,
        });

        string? name = export.Departments?[0].Objects?[0].Name;
        Assert.False(string.IsNullOrWhiteSpace(name));
        Assert.DoesNotContain('?', name);
    }
}
