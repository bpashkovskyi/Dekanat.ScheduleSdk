using Dekanat.ScheduleSdk.Models;

namespace Dekanat.ScheduleSdk.Tests;

/// <summary>
/// Логіка моделі <see cref="PsRozkladExport"/> (успіх, коди помилок).
/// </summary>
[Trait("Category", "Unit")]
public sealed class PsRozkladExportModelTests
{
    [Fact]
    public void IsSuccess_WhenCodeZeroAndNoError_ReturnsTrue()
    {
        PsRozkladExport export = new() { Code = 0 };
        Assert.True(export.IsSuccess);
    }

    [Fact]
    public void IsSuccess_WhenErrorPresent_ReturnsFalse()
    {
        PsRozkladExport export = new()
        {
            Code = 0,
            Error = new ApiErrorDetails { Message = "err", ErrorCode = -1 },
        };

        Assert.False(export.IsSuccess);
    }

    [Theory]
    [InlineData(-90, -90, -90)]
    [InlineData(90, null, -90)]
    [InlineData(0, null, 0)]
    public void GetNormalizedErrorCode_PrefersErrorObject(int? code, int? errorCode, int? expected)
    {
        PsRozkladExport export = new()
        {
            Code = code,
            Error = errorCode is null
                ? null
                : new ApiErrorDetails { ErrorCode = errorCode },
        };

        Assert.Equal(expected, export.GetNormalizedErrorCode());
    }
}
