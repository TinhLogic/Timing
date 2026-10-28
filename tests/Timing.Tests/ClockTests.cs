using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Timing.Tests;

public sealed class ClockTests
{
    [Fact]
    public void DefaultRegistration_UsesUtc()
    {
        // Sắp xếp
        // Arrange
        var services = new ServiceCollection();
        services.AddTimingClock();
        var provider = services.BuildServiceProvider();

        // Hành động
        // Act
        var clock = provider.GetRequiredService<IClock>();

        // Khẳng định
        // Assert
        Assert.Equal(DateTimeKind.Utc, clock.Kind);
        Assert.True(clock.SupportsMultipleTimezone);
        Assert.Equal(TimeSpan.Zero, clock.NowOffset.Offset);
    }

    [Fact]
    public void LocalRegistration_UsesLocalKind()
    {
        // Sắp xếp
        // Arrange
        var services = new ServiceCollection();
        services.AddTimingClock(options =>
        {
            options.Kind = DateTimeKind.Local;
        });
        var provider = services.BuildServiceProvider();

        // Hành động
        // Act
        var clock = provider.GetRequiredService<IClock>();

        // Khẳng định
        // Assert
        Assert.Equal(DateTimeKind.Local, clock.Kind);
        Assert.False(clock.SupportsMultipleTimezone);
    }

    [Fact]
    public void NormalizeDateTime_WhenUtcConfigured_ConvertsLocalToUtc()
    {
        // Sắp xếp
        // Arrange
        var services = new ServiceCollection();
        services.AddTimingClock(options => options.Kind = DateTimeKind.Utc);
        var provider = services.BuildServiceProvider();
        var clock = provider.GetRequiredService<IClock>();

        var localTime = DateTime.Now;

        // Hành động
        // Act
        var normalized = clock.Normalize(localTime);

        // Khẳng định
        // Assert
        Assert.Equal(DateTimeKind.Utc, normalized.Kind);
        Assert.Equal(localTime.ToUniversalTime(), normalized);
    }

    [Fact]
    public void NormalizeDateTime_WhenLocalConfigured_ConvertsUtcToLocal()
    {
        // Sắp xếp
        // Arrange
        var services = new ServiceCollection();
        services.AddTimingClock(options => options.Kind = DateTimeKind.Local);
        var provider = services.BuildServiceProvider();
        var clock = provider.GetRequiredService<IClock>();

        var utcTime = DateTime.UtcNow;

        // Hành động
        // Act
        var normalized = clock.Normalize(utcTime);

        // Khẳng định
        // Assert
        Assert.Equal(DateTimeKind.Local, normalized.Kind);
        Assert.Equal(utcTime.ToLocalTime(), normalized);
    }

    [Fact]
    public void NormalizeDateTimeOffset_WhenUtcConfigured_NormalizesToUtcOffset()
    {
        // Sắp xếp
        // Arrange
        var services = new ServiceCollection();
        services.AddTimingClock(options => options.Kind = DateTimeKind.Utc);
        var provider = services.BuildServiceProvider();
        var clock = provider.GetRequiredService<IClock>();

        var offset = DateTimeOffset.Now;

        // Hành động
        // Act
        var normalized = clock.Normalize(offset);

        // Khẳng định
        // Assert
        Assert.Equal(TimeSpan.Zero, normalized.Offset);
        Assert.Equal(offset.ToUniversalTime(), normalized);
    }

#if NET8_0_OR_GREATER
    [Fact]
    public void CustomTimeProvider_ReturnsConfiguredFixedTime()
    {
        // Sắp xếp
        // Arrange
        var fixedTime = new DateTimeOffset(2026, 10, 28, 12, 0, 0, TimeSpan.Zero);
        var fakeProvider = new FixedTimeProvider(fixedTime);

        var services = new ServiceCollection();
        services.AddTimingClock(options =>
        {
            options.Kind = DateTimeKind.Utc;
            options.TimeProvider = fakeProvider;
        });

        var provider = services.BuildServiceProvider();

        // Hành động
        // Act
        var clock = provider.GetRequiredService<IClock>();

        // Khẳng định
        // Assert
        Assert.Equal(fixedTime, clock.NowOffset);
        Assert.Equal(fixedTime.UtcDateTime, clock.Now);
        Assert.Same(fakeProvider, clock.TimeProvider);
    }

    private sealed class FixedTimeProvider : TimeProvider
    {
        private readonly DateTimeOffset _utcNow;

        public FixedTimeProvider(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }

        public override DateTimeOffset GetUtcNow() => _utcNow;
    }
#endif
}

