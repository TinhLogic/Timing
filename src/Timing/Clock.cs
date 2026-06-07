using Microsoft.Extensions.Options;

namespace Timing;

internal sealed class Clock : IClock
{
    private ClockOptions Options { get; }

    public Clock(IOptions<ClockOptions> options)
    {
        Options = options.Value;
    }

    // Trả về DateTimeKind đang được cấu hình
    public DateTimeKind Kind => Options.Kind;

    // Bật cờ hỗ trợ đa múi giờ nếu cấu hình là UTC
    public bool SupportsMultipleTimezone => Options.Kind == DateTimeKind.Utc;

    public DateTime Now => Options.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;

    public DateTimeOffset NowOffset => Options.Kind == DateTimeKind.Utc ? DateTimeOffset.UtcNow : DateTimeOffset.Now;

    // Hàm chuẩn hóa thời gian
    public DateTime Normalize(DateTime dateTime)
    {
        // Nếu không cấu hình Kind, hoặc Kind truyền vào đã khớp với cấu hình -> Giữ nguyên
        if (Kind == DateTimeKind.Unspecified || Kind == dateTime.Kind)
        {
            return dateTime;
        }

        // Nếu hệ thống dùng Local, nhưng dữ liệu vào là UTC -> Convert sang Local
        if (Kind == DateTimeKind.Local && dateTime.Kind == DateTimeKind.Utc)
        {
            return dateTime.ToLocalTime();
        }

        // Nếu hệ thống dùng UTC, nhưng dữ liệu vào là Local -> Convert sang UTC
        if (Kind == DateTimeKind.Utc && dateTime.Kind == DateTimeKind.Local)
        {
            return dateTime.ToUniversalTime();
        }

        // Các trường hợp Unspecified khác -> Ép cứng Kind theo cấu hình hệ thống
        return DateTime.SpecifyKind(dateTime, Kind);
    }

    // Hàm chuẩn hóa thời gian
    public DateTimeOffset Normalize(DateTimeOffset dateTime)
    {
        // Nếu không cấu hình Kind -> Giữ nguyên
        if (Kind == DateTimeKind.Unspecified)
        {
            return dateTime;
        }

        // Nếu hệ thống dùng Local -> Convert sang Local
        if (Kind == DateTimeKind.Local)
        {
            return dateTime.ToLocalTime();
        }

        // Nếu hệ thống dùng UTC -> Convert sang UTC
        if (Kind == DateTimeKind.Utc)
        {
            return dateTime.ToUniversalTime();
        }

        return dateTime;
    }
}
