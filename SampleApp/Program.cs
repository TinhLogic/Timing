using System;
using Microsoft.Extensions.DependencyInjection;
using Timing;

var services = new ServiceCollection();
services.AddTimingClock(options =>
{
    options.Kind = DateTimeKind.Utc;
});

var serviceProvider = services.BuildServiceProvider();
var clock = serviceProvider.GetRequiredService<IClock>();

Console.WriteLine("=== Timing Sample App (.NET 8 with TimeProvider) ===");
Console.WriteLine();
Console.WriteLine($"Now (DateTime): {clock.Now:yyyy-MM-dd HH:mm:ss.fff}");
Console.WriteLine($"NowOffset: {clock.NowOffset:yyyy-MM-dd HH:mm:ss.fff zzz}");
Console.WriteLine($"Supports Multiple Timezone: {clock.SupportsMultipleTimezone}");
Console.WriteLine($"Kind: {clock.Kind}");

#if NET8_0_OR_GREATER
Console.WriteLine($"TimeProvider Type: {clock.TimeProvider.GetType().FullName}");
Console.WriteLine($"TimeProvider LocalTimeZone: {clock.TimeProvider.LocalTimeZone.DisplayName}");
#endif

Console.WriteLine();

var now = DateTime.Now;
var normalized = clock.Normalize(now);
Console.WriteLine($"Input DateTime: {now} (Kind: {now.Kind})");
Console.WriteLine($"Normalized DateTime: {normalized} (Kind: {normalized.Kind})");

var offsetNow = DateTimeOffset.Now;
var normalizedOffset = clock.Normalize(offsetNow);
Console.WriteLine($"Input DateTimeOffset: {offsetNow}");
Console.WriteLine($"Normalized DateTimeOffset (UTC): {normalizedOffset}");
