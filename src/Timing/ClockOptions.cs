namespace Timing;

/// <summary>
/// Cấu hình tùy chọn cho dịch vụ Clock.
/// <para>Configuration options for the Clock service.</para>
/// </summary>
public sealed class ClockOptions
{
    /// <summary>
    /// Kiểu <see cref="DateTimeKind"/> được cấu hình cho Clock (Utc hoặc Local). Mặc định là Utc.
    /// <para>The <see cref="DateTimeKind"/> configured for the Clock (Utc or Local). Defaults to Utc.</para>
    /// </summary>
    public DateTimeKind Kind { get; set; } = DateTimeKind.Utc;

#if NET8_0_OR_GREATER
    /// <summary>
    /// <see cref="System.TimeProvider"/> dùng để cung cấp thời gian. Mặc định là null (sẽ fallback về DI hoặc TimeProvider.System).
    /// <para><see cref="System.TimeProvider"/> used to provide time. Defaults to null (will fallback to DI or TimeProvider.System).</para>
    /// </summary>
    public TimeProvider? TimeProvider { get; set; }
#endif
}
