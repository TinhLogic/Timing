namespace Timing;

/// <summary>
/// Giao diện dịch vụ Clock dùng để truy xuất và chuẩn hóa thời gian trên toàn ứng dụng.
/// <para>Clock service interface used to retrieve and normalize time across the application.</para>
/// </summary>
public interface IClock
{
    /// <summary>
    /// Lấy thời gian hiện tại theo kiểu clock đã cấu hình (UTC hoặc Local).
    /// <para>Gets the current local or UTC time based on the configured clock kind.</para>
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Lấy thời điểm hiện tại dưới dạng <see cref="DateTimeOffset"/>, giữ nguyên offset.
    /// <para>Gets the current moment as a <see cref="DateTimeOffset"/>, preserving the offset.</para>
    /// </summary>
    DateTimeOffset NowOffset { get; }

    /// <summary>
    /// Chuẩn hóa giá trị <see cref="DateTime"/> theo <see cref="DateTimeKind"/> đã cấu hình.
    /// <para>Normalizes a <see cref="DateTime"/> value to the configured <see cref="DateTimeKind"/>.</para>
    /// </summary>
    /// <param name="dateTime">Giá trị ngày giờ đầu vào cần chuẩn hóa. / The input date and time to normalize.</param>
    /// <returns>Giá trị <see cref="DateTime"/> đã được chuẩn hóa. / The normalized <see cref="DateTime"/>.</returns>
    DateTime Normalize(DateTime dateTime);

    /// <summary>
    /// Chuẩn hóa giá trị <see cref="DateTimeOffset"/> theo kiểu clock và offset đã cấu hình.
    /// <para>Normalizes a <see cref="DateTimeOffset"/> value to the configured clock kind and offset.</para>
    /// </summary>
    /// <param name="dateTime">Giá trị ngày giờ có offset đầu vào cần chuẩn hóa. / The input date and time offset to normalize.</param>
    /// <returns>Giá trị <see cref="DateTimeOffset"/> đã được chuẩn hóa. / The normalized <see cref="DateTimeOffset"/>.</returns>
    DateTimeOffset Normalize(DateTimeOffset dateTime);

    /// <summary>
    /// Cho biết clock có hỗ trợ nhiều múi giờ hoặc nhiều offset hay không.
    /// <para>Indicates whether the clock supports multiple time zones or offsets.</para>
    /// </summary>
    bool SupportsMultipleTimezone { get; }

    /// <summary>
    /// Lấy <see cref="DateTimeKind"/> đã cấu hình cho clock này.
    /// <para>Gets the configured <see cref="DateTimeKind"/> used by this clock.</para>
    /// </summary>
    DateTimeKind Kind { get; }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Lấy <see cref="System.TimeProvider"/> đang được sử dụng bởi clock này.
    /// <para>Gets the <see cref="System.TimeProvider"/> used by this clock.</para>
    /// </summary>
    TimeProvider TimeProvider => TimeProvider.System;
#endif
}
