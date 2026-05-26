namespace Dekanat.ScheduleSdk.IntegrationTests;

/// <summary>
/// Спільні налаштування інтеграційних тестів (один екземпляр HTTP-клієнта на колекцію).
/// </summary>
[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<PsRozkladClientFixture>
{
    public const string Name = "PsRozkladApi";
}
