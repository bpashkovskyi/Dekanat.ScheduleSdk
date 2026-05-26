using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.Options;

namespace Dekanat.ScheduleSdk.IntegrationTests;

/// <summary>
/// Фабрика клієнта для інтеграційних тестів проти реального API dekanat.nung.edu.ua.
/// </summary>
public sealed class PsRozkladClientFixture : IDisposable
{
    public PsRozkladClientFixture()
    {
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri(PsRozkladClientOptions.DefaultBaseUrl),
            Timeout = TimeSpan.FromSeconds(90),
        };

        Client = new PsRozkladClient(httpClient, new PsRozkladClientOptions
        {
            Encoding = TextEncodingMode.Utf8,
            ThrowOnApiError = true,
        });
    }

    public PsRozkladClient Client { get; }

    public void Dispose()
    {
        // HttpClient створено вручну — в тестах достатньо завершення процесу.
    }
}
