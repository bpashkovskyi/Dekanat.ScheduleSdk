using Dekanat.ScheduleSdk.Options;

namespace Dekanat.ScheduleSdk.Tests.Helpers;

/// <summary>
/// Створює <see cref="PsRozkladClient"/> з mock HTTP для юніт-тестів.
/// </summary>
internal static class PsRozkladClientTestFactory
{
    public static (PsRozkladClient Client, RecordingHttpMessageHandler Handler) Create(
        PsRozkladClientOptions? options = null,
        string? responseJson = null)
    {
        RecordingHttpMessageHandler handler = new();
        if (responseJson is not null)
        {
            handler.ResponseContent = responseJson;
        }

        HttpClient httpClient = new(handler)
        {
            BaseAddress = new Uri(PsRozkladClientOptions.DefaultBaseUrl),
        };

        PsRozkladClientOptions clientOptions = options ?? new PsRozkladClientOptions();
        PsRozkladClient client = PsRozkladClient.Create(httpClient, clientOptions);
        return (client, handler);
    }
}
