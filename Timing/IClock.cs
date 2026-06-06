namespace Timing;

public interface IClock
{
    /// <summary>
    /// Lấy thời gian hiện tại theo kiểu clock đã cấu hình (UTC hoặc Local).
    /// Gets the current local or UTC time based on the configured clock kind.
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Lấy thời điểm hiện tại dưới dạng <see cref="DateTimeOffset"/>, giữ nguyên offset.
    /// Gets the current moment as a <see cref="DateTimeOffset"/>, preserving the offset.
    /// </summary>
    DateTimeOffset NowOffset { get; }

    /// <summary>
    /// Chuẩn hóa giá trị <see cref="DateTime"/> theo <see cref="DateTimeKind"/> đã cấu hình.
    /// Normalizes a <see cref="DateTime"/> value to the configured <see cref="DateTimeKind"/>.
    /// </summary>
    /// <param name="dateTime">Giá trị ngày giờ đầu vào cần chuẩn hóa. / The input date and time to normalize.</param>
    /// <returns>Giá trị <see cref="DateTime"/> đã được chuẩn hóa. / The normalized <see cref="DateTime"/>.</returns>
    DateTime Normalize(DateTime dateTime);

    /// <summary>
    /// Chuẩn hóa giá trị <see cref="DateTimeOffset"/> theo kiểu clock và offset đã cấu hình.
    /// Normalizes a <see cref="DateTimeOffset"/> value to the configured clock kind and offset.
    /// </summary>
    /// <param name="dateTime">Giá trị ngày giờ có offset đầu vào cần chuẩn hóa. / The input date and time offset to normalize.</param>
    /// <returns>Giá trị <see cref="DateTimeOffset"/> đã được chuẩn hóa. / The normalized <see cref="DateTimeOffset"/>.</returns>
    DateTimeOffset Normalize(DateTimeOffset dateTime);

    /// <summary>
    /// Cho biết clock có hỗ trợ nhiều múi giờ hoặc nhiều offset hay không.
    /// Indicates whether the clock supports multiple time zones or offsets.
    /// </summary>
    bool SupportsMultipleTimezone { get; }

    /// <summary>
    /// Lấy <see cref="DateTimeKind"/> đã cấu hình cho clock này.
    /// Gets the configured <see cref="DateTimeKind"/> used by this clock.
    /// </summary>
    DateTimeKind Kind { get; }
}