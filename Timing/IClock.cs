
namespace Timing;

public interface IClock
{
    DateTime Now { get; }

    DateTimeOffset NowOffset { get; }

    DateTime Normalize(DateTime dateTime);

    DateTimeOffset Normalize(DateTimeOffset dateTime);

    bool SupportsMultipleTimezone { get; }

    DateTimeKind Kind { get; }
}