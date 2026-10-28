using System;
using Microsoft.Extensions.DependencyInjection;
#if NET8_0_OR_GREATER
using Microsoft.Extensions.DependencyInjection.Extensions;
#endif

namespace Timing;

/// <summary>
/// Phương thức mở rộng để đăng ký dịch vụ Timing Clock vào <see cref="IServiceCollection"/>.
/// <para>Extension methods for registering Timing Clock services into <see cref="IServiceCollection"/>.</para>
/// </summary>
public static class ClockServiceCollectionExtensions
{
    /// <summary>
    /// Đăng ký dịch vụ Timing Clock vào ServiceCollection.
    /// <para>Registers Timing Clock services into the ServiceCollection.</para>
    /// </summary>
    /// <param name="services">Tập hợp các dịch vụ DI. / The service collection.</param>
    /// <param name="configureOptions">Hành động cấu hình ClockOptions tùy chọn. / Optional action to configure ClockOptions.</param>
    /// <returns>Tập hợp các dịch vụ DI để tiếp tục cấu hình theo chuỗi. / The service collection for chaining.</returns>
    public static IServiceCollection AddTimingClock(this IServiceCollection services, Action<ClockOptions>? configureOptions = null)
    {
        services.AddOptions<ClockOptions>();

        if (configureOptions is not null)
        {
            services.Configure(configureOptions);
        }
        else
        {
            services.Configure<ClockOptions>(options => options.Kind = DateTimeKind.Utc);
        }

#if NET8_0_OR_GREATER
        // Đăng ký TimeProvider.System nếu DI container chưa có TimeProvider nào khác
        // Register TimeProvider.System if no other TimeProvider has been registered in the DI container
        services.TryAddSingleton(TimeProvider.System);
#endif

        services.AddSingleton<IClock, Clock>();
        return services;
    }
}
