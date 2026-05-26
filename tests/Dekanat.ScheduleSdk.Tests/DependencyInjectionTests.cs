using Dekanat.ScheduleSdk.DependencyInjection;
using Dekanat.ScheduleSdk.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Dekanat.ScheduleSdk.Tests;

/// <summary>
/// Перевірка реєстрації в DI без неоднозначності конструкторів.
/// </summary>
[Trait("Category", "Unit")]
public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddPsRozkladClient_ResolvesSingleClientInstance()
    {
        ServiceCollection services = new();
        services.AddPsRozkladClient(options =>
        {
            options.BaseUrl = new Uri(PsRozkladClientOptions.DefaultBaseUrl);
        });

        ServiceProvider provider = services.BuildServiceProvider();
        IPsRozkladClient client = provider.GetRequiredService<IPsRozkladClient>();

        Assert.IsType<PsRozkladClient>(client);
    }
}
