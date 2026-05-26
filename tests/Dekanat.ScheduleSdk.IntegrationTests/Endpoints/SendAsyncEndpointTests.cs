using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.IntegrationTests.Helpers;
using Dekanat.ScheduleSdk.Models;
using Dekanat.ScheduleSdk.Options;

namespace Dekanat.ScheduleSdk.IntegrationTests.Endpoints;

/// <summary>
/// Низькорівневий <see cref="IPsRozkladClient.SendAsync"/> та опції клієнта.
/// </summary>
[Collection(IntegrationTestCollection.Name)]
[Trait("Category", "Integration")]
public sealed class SendAsyncEndpointTests
{
    [Fact]
    public async Task SendAsync_RawObjectList_ReturnsParsedResponse()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        using HttpClient httpClient = new()
        {
            BaseAddress = new Uri(PsRozkladClientOptions.DefaultBaseUrl),
            Timeout = TimeSpan.FromSeconds(90),
        };

        PsRozkladClient client = PsRozkladClient.Create(httpClient, new PsRozkladClientOptions());

        PsRozkladResponse response = await client.SendAsync(new Dictionary<string, string>
        {
            ["req_mode"] = "group",
            ["req_type"] = "obj_list",
            ["show_ID"] = "yes",
        });

        Assert.NotNull(response.Export);
        Assert.True(response.Export.IsSuccess);
        Assert.NotNull(response.Export.Departments);
    }

    [Fact]
    public async Task SendAsync_WithoutThrowOnError_ReturnsErrorObject()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        using HttpClient httpClient = new()
        {
            BaseAddress = new Uri(PsRozkladClientOptions.DefaultBaseUrl),
            Timeout = TimeSpan.FromSeconds(90),
        };

        PsRozkladClient client = PsRozkladClient.Create(httpClient, new PsRozkladClientOptions
        {
            ThrowOnApiError = false,
        });

        PsRozkladResponse response = await client.SendAsync(new Dictionary<string, string>
        {
            ["req_mode"] = "teacher",
            ["req_type"] = "rozklad",
            ["OBJ_ID"] = "999999999",
            ["begin_date"] = "01.02.2026",
            ["end_date"] = "02.02.2026",
        });

        Assert.False(response.Export!.IsSuccess);
        Assert.NotNull(response.Export.Error);
    }

    [Fact]
    public async Task SendAsync_Windows1251Encoding_ReturnsReadableText()
    {
        if (!IntegrationTestContext.IsEnabled)
        {
            return;
        }

        using HttpClient httpClient = new()
        {
            BaseAddress = new Uri(PsRozkladClientOptions.DefaultBaseUrl),
            Timeout = TimeSpan.FromSeconds(90),
        };

        PsRozkladClient client = PsRozkladClient.Create(httpClient, new PsRozkladClientOptions
        {
            Encoding = TextEncodingMode.Windows1251,
        });

        PsRozkladResponse response = await client.SendAsync(
            new Dictionary<string, string>
            {
                ["req_mode"] = "group",
                ["req_type"] = "obj_list",
            },
            TextEncodingMode.Windows1251);

        string? name = response.Export?.Departments?[0].Objects?[0].Name;
        Assert.False(string.IsNullOrWhiteSpace(name));
        Assert.DoesNotContain('?', name);
    }
}
