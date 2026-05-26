namespace Nung.ScheduleSdk.IntegrationTests;

/// <summary>
/// Дозволяє пропустити мережеві тести через змінну середовища <c>SKIP_PSROZKLAD_INTEGRATION_TESTS=1</c>.
/// </summary>
internal static class IntegrationTestSkip
{
    public static bool IsEnabled =>
        !string.Equals(
            Environment.GetEnvironmentVariable("SKIP_PSROZKLAD_INTEGRATION_TESTS"),
            "1",
            StringComparison.Ordinal);
}
