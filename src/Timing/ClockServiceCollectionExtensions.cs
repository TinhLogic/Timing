using System;
using Microsoft.Extensions.DependencyInjection;

namespace Timing;

public static class ClockServiceCollectionExtensions
{
    public static IServiceCollection AddTimingClock(this IServiceCollection services, Action<ClockOptions>? configureOptions = null)
    {
        services.AddOptions<ClockOptions>();

        if (configureOptions is not null)
        {
            services.Configure<ClockOptions>(configureOptions);
        }
        else
        {
            services.Configure<ClockOptions>(options => options.Kind = DateTimeKind.Utc);
        }

        services.AddSingleton<IClock, Clock>();
        return services;
    }
}
