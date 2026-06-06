using System;
using Microsoft.Extensions.DependencyInjection;
using Timing;

var services = new ServiceCollection();
services.AddTimingClock(options =>
{
    options.Kind = DateTimeKind.Local;
});

var serviceProvider = services.BuildServiceProvider();
var clock = serviceProvider.GetRequiredService<IClock>();

Console.WriteLine("=== Timing Sample App ===");
Console.WriteLine();
Console.WriteLine($"Now (DateTime): {clock.Now}");
Console.WriteLine($"NowOffset: {clock.NowOffset}");
// Console.WriteLine($"SupportsMultipleTimezone: {clock.SupportsMultipleTimezone}");
// Console.WriteLine($"Kind: {clock.Kind}");
Console.WriteLine();

var now = DateTime.Now;
var normalized = clock.Normalize(now);
Console.WriteLine($"Normalized DateTime (UTC): {normalized}");

var offsetNow = DateTimeOffset.Now;
var normalizedOffset = clock.Normalize(offsetNow);
Console.WriteLine($"Normalized DateTimeOffset (UTC): {normalizedOffset}");


