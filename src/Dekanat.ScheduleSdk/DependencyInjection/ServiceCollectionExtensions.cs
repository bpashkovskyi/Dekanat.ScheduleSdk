using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Dekanat.ScheduleSdk.Options;

namespace Dekanat.ScheduleSdk.DependencyInjection;

/// <summary>
/// Розширення для реєстрації <see cref="IPsRozkladClient"/> у контейнері DI.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Реєструє типізований HTTP-клієнт <see cref="PsRozkladClient"/> та опції <see cref="PsRozkladClientOptions"/>.
    /// </summary>
    /// <param name="services">Колекція сервісів ASP.NET Core / Generic Host.</param>
    /// <param name="configureOptions">Необов'язкове налаштування базової URL, кодування та таймауту.</param>
    public static IServiceCollection AddPsRozkladClient(
        this IServiceCollection services,
        Action<PsRozkladClientOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (configureOptions is not null)
        {
            services.Configure(configureOptions);
        }
        else
        {
            services.AddOptions<PsRozkladClientOptions>();
        }

        services.AddHttpClient<IPsRozkladClient, PsRozkladClient>((serviceProvider, httpClient) =>
        {
            PsRozkladClientOptions options = serviceProvider
                .GetRequiredService<IOptions<PsRozkladClientOptions>>()
                .Value;

            httpClient.BaseAddress = options.BaseUrl;
            httpClient.Timeout = options.RequestTimeout;
        });

        return services;
    }
}
