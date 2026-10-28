using Microsoft.Extensions.Options;

namespace Timing;

internal sealed class Clock : IClock
{
    private readonly ClockOptions _options;

#if NET8_0_OR_GREATER
    private readonly TimeProvider _timeProvider;

    public Clock(IOptions<ClockOptions> options, TimeProvider? timeProvider = null)
    {
        _options = options.Value;
        // Ưu tiên TimeProvider từ Options, tiếp theo là injected từ DI, cuối cùng fallback về TimeProvider.System
        // Prioritize TimeProvider from Options, then injected from DI, finally fallback to TimeProvider.System
        _timeProvider = _options.TimeProvider ?? timeProvider ?? TimeProvider.System;
    }

    public TimeProvider TimeProvider => _timeProvider;

    // Trả về DateTimeKind đang được cấu hình
    // Returns the currently configured DateTimeKind
    public DateTimeKind Kind => _options.Kind;

    // Bật cờ hỗ trợ đa múi giờ nếu cấu hình là UTC
    // Enables multi-timezone support flag if configured as UTC
    public bool SupportsMultipleTimezone => _options.Kind == DateTimeKind.Utc;

    public DateTime Now => _options.Kind == DateTimeKind.Utc
        ? _timeProvider.GetUtcNow().UtcDateTime
        : _timeProvider.GetLocalNow().LocalDateTime;

    public DateTimeOffset NowOffset => _options.Kind == DateTimeKind.Utc
        ? _timeProvider.GetUtcNow()
        : _timeProvider.GetLocalNow();
#else
    public Clock(IOptions<ClockOptions> options)
    {
        _options = options.Value;
    }

    // Trả về DateTimeKind đang được cấu hình
    // Returns the currently configured DateTimeKind
    public DateTimeKind Kind => _options.Kind;

    // Bật cờ hỗ trợ đa múi giờ nếu cấu hình là UTC
    // Enables multi-timezone support flag if configured as UTC
    public bool SupportsMultipleTimezone => _options.Kind == DateTimeKind.Utc;

    public DateTime Now => _options.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;

    public DateTimeOffset NowOffset => _options.Kind == DateTimeKind.Utc ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
#endif

    // Hàm chuẩn hóa thời gian DateTime
    // Normalizes DateTime value
    public DateTime Normalize(DateTime dateTime)
    {
        // Nếu không cấu hình Kind, hoặc Kind truyền vào đã khớp với cấu hình -> Giữ nguyên
        // If Kind is Unspecified, or input Kind matches configured Kind -> Keep as-is
        if (Kind == DateTimeKind.Unspecified || Kind == dateTime.Kind)
        {
            return dateTime;
        }

        // Nếu hệ thống dùng Local, nhưng dữ liệu vào là UTC -> Convert sang Local
        // If system uses Local, but input is UTC -> Convert to Local
        if (Kind == DateTimeKind.Local && dateTime.Kind == DateTimeKind.Utc)
        {
            return dateTime.ToLocalTime();
        }

        // Nếu hệ thống dùng UTC, nhưng dữ liệu vào là Local -> Convert sang UTC
        // If system uses UTC, but input is Local -> Convert to UTC
        if (Kind == DateTimeKind.Utc && dateTime.Kind == DateTimeKind.Local)
        {
            return dateTime.ToUniversalTime();
        }

        // Các trường hợp Unspecified khác -> Ép cứng Kind theo cấu hình hệ thống
        // Other Unspecified cases -> Specify Kind according to system configuration
        return DateTime.SpecifyKind(dateTime, Kind);
    }

    // Hàm chuẩn hóa thời gian DateTimeOffset
    // Normalizes DateTimeOffset value
    public DateTimeOffset Normalize(DateTimeOffset dateTime)
    {
        // Nếu không cấu hình Kind -> Giữ nguyên
        // If Kind is Unspecified -> Keep as-is
        if (Kind == DateTimeKind.Unspecified)
        {
            return dateTime;
        }

        // Nếu hệ thống dùng Local -> Convert sang Local
        // If system uses Local -> Convert to Local
        if (Kind == DateTimeKind.Local)
        {
            return dateTime.ToLocalTime();
        }

        // Nếu hệ thống dùng UTC -> Convert sang UTC
        // If system uses UTC -> Convert to UTC
        if (Kind == DateTimeKind.Utc)
        {
            return dateTime.ToUniversalTime();
        }

        return dateTime;
    }
}
