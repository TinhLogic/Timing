# Timing

[![English](https://img.shields.io/badge/English-below-green)](https://github.com/TinhLogic/Timing#english) [![NuGet](https://img.shields.io/nuget/v/Timing.Net.svg?label=NuGet)](https://www.nuget.org/packages/Timing.Net/1.1.0)

Thư viện nhỏ cho .NET hỗ trợ đồng thời **.NET 6** và **.NET 8+** để xử lý thời gian toàn ứng dụng với hỗ trợ chuẩn hóa UTC/local, tích hợp **`TimeProvider`** (.NET 8+) và đăng ký dependency injection.

**`IClock`** là một service cốt lõi để quản lý các thao tác liên quan đến thời gian (ngày/giờ).

Mục đích chính của việc sử dụng `IClock` thay vì gọi trực tiếp `DateTime.Now` hoặc `DateTime.UtcNow` là để **đảm bảo tính nhất quán về `DateTimeKind`** (UTC, Local, hoặc Unspecified) trên toàn bộ ứng dụng của bạn. Điều này đặc biệt quan trọng khi hệ thống phải phục vụ người dùng ở nhiều múi giờ khác nhau.

Lợi ích:
- **Consistency**: Tất cả thời gian trong ứng dụng tuân theo một cấu hình `DateTimeKind` duy nhất
- **TimeProvider Integration**: Tích hợp chuẩn `TimeProvider` của .NET 8, dễ dàng thay thế `FakeTimeProvider` khi viết unit test
- **Testability**: Có thể mock `IClock` hoặc `TimeProvider` trong unit test mà không phụ thuộc thời gian hệ thống thật
- **Centralized Management**: Quản lý cách xử lý thời gian tại một điểm duy nhất
- **Multi-targeting**: Hỗ trợ cả `.NET 6.0` và `.NET 8.0` trong cùng một NuGet package

## Tính năng

- Trừu tượng `IClock` để lấy thời gian hiện tại
- Hỗ trợ `DateTime` và `DateTimeOffset`
- Chuẩn hóa theo `DateTimeKind` được cấu hình (UTC / Local)
- Tích hợp `System.TimeProvider` gốc trên .NET 8+
- Tương thích ngược hoàn toàn với .NET 6
- Đăng ký vào `ServiceCollection` qua `AddTimingClock`
- Lớp `Clock` là internal sealed, chỉ lộ ra API công khai `IClock`

## Cài đặt qua NuGet

```bash
dotnet add package Timing.Net --version 1.1.0
```

## Sử dụng

### Đăng ký clock trong DI

```csharp
using Microsoft.Extensions.DependencyInjection;
using Timing;

var services = new ServiceCollection();
services.AddTimingClock(options =>
{
    options.Kind = DateTimeKind.Utc; // hoặc DateTimeKind.Local
});

var serviceProvider = services.BuildServiceProvider();
```

Mặc định, nếu không truyền tùy chọn thì clock sẽ dùng `DateTimeKind.Utc`.

### Lấy và dùng `IClock`

```csharp
var clock = serviceProvider.GetRequiredService<IClock>();

DateTime currentDateTime = clock.Now;
DateTimeOffset currentOffset = clock.NowOffset;

DateTime normalizedDateTime = clock.Normalize(DateTime.Now);
DateTimeOffset normalizedOffset = clock.Normalize(DateTimeOffset.Now);
```

### Sử dụng TimeProvider (.NET 8+)

Trên .NET 8+, bạn có thể truy xuất `TimeProvider` trực tiếp từ `IClock` hoặc cấu hình `TimeProvider` tùy chỉnh:

```csharp
// Truy xuất TimeProvider từ clock
TimeProvider provider = clock.TimeProvider;
DateTimeOffset utcNow = provider.GetUtcNow();

// Cấu hình TimeProvider tùy chỉnh (hoặc FakeTimeProvider cho testing)
services.AddTimingClock(options =>
{
    options.Kind = DateTimeKind.Utc;
    options.TimeProvider = myCustomTimeProvider;
});
```

### Hành vi của clock

- `Now` trả về thời gian hiện tại dưới dạng `DateTime`
- `NowOffset` trả về thời gian hiện tại dưới dạng `DateTimeOffset`
- `Normalize(DateTime)` chuẩn hóa đầu vào theo `DateTimeKind` đã cấu hình
- `Normalize(DateTimeOffset)` chuyển đầu vào về UTC hoặc local tuỳ cấu hình
- `TimeProvider` (.NET 8+) trả về `TimeProvider` đang được sử dụng (mặc định là `TimeProvider.System`)

## API

- `IClock`
  - `DateTime Now { get; }`
  - `DateTimeOffset NowOffset { get; }`
  - `DateTime Normalize(DateTime dateTime)`
  - `DateTimeOffset Normalize(DateTimeOffset dateTime)`
  - `bool SupportsMultipleTimezone { get; }`
  - `DateTimeKind Kind { get; }`
  - `TimeProvider TimeProvider { get; }` *(.NET 8+)*
- `ClockOptions`
  - `DateTimeKind Kind { get; set; }`
  - `TimeProvider? TimeProvider { get; set; }` *(.NET 8+)*
- `ClockServiceCollectionExtensions`
  - `AddTimingClock(this IServiceCollection services, Action<ClockOptions>? configureOptions = null)`

---

# English

> **Note**: This English section is automatically translated from Vietnamese by AI.

A lightweight .NET library supporting both **.NET 6** and **.NET 8+** for handling application-wide time management with UTC/local normalization support, **`TimeProvider`** integration (.NET 8+), and dependency injection registration.

**`IClock`** is a core service for managing time-related operations (date/time).

The main purpose of using `IClock` instead of directly calling `DateTime.Now` or `DateTime.UtcNow` is to **ensure consistency in `DateTimeKind`** (UTC, Local, or Unspecified) throughout your application. This is particularly important when the system needs to serve users in multiple different time zones.

Benefits:
- **Consistency**: All time in the application follows a single `DateTimeKind` configuration
- **TimeProvider Integration**: Native `TimeProvider` integration on .NET 8+, easy to swap with `FakeTimeProvider` for unit testing
- **Testability**: Can mock `IClock` or `TimeProvider` in unit tests without depending on the actual system clock
- **Centralized Management**: Manage how time is handled at a single point
- **Multi-targeting**: Supports both `.NET 6.0` and `.NET 8.0` in the same NuGet package

## Features

- `IClock` abstraction for getting the current time
- Support for `DateTime` and `DateTimeOffset`
- Normalization based on configured `DateTimeKind` (UTC / Local)
- Native `System.TimeProvider` integration on .NET 8+
- Full backward compatibility with .NET 6
- Registration into `ServiceCollection` via `AddTimingClock`
- `Clock` class is internal sealed, only exposing the public `IClock` API

## Installation via NuGet

```bash
dotnet add package Timing.Net --version 1.1.0
```

## Usage

### Register clock in DI

```csharp
using Microsoft.Extensions.DependencyInjection;
using Timing;

var services = new ServiceCollection();
services.AddTimingClock(options =>
{
    options.Kind = DateTimeKind.Utc; // or DateTimeKind.Local
});

var serviceProvider = services.BuildServiceProvider();
```

By default, if no options are provided, the clock will use `DateTimeKind.Utc`.

### Get and use `IClock`

```csharp
var clock = serviceProvider.GetRequiredService<IClock>();

DateTime currentDateTime = clock.Now;
DateTimeOffset currentOffset = clock.NowOffset;

DateTime normalizedDateTime = clock.Normalize(DateTime.Now);
DateTimeOffset normalizedOffset = clock.Normalize(DateTimeOffset.Now);
```

### Using TimeProvider (.NET 8+)

On .NET 8+, you can access `TimeProvider` directly from `IClock` or configure a custom `TimeProvider`:

```csharp
// Access TimeProvider from clock
TimeProvider provider = clock.TimeProvider;
DateTimeOffset utcNow = provider.GetUtcNow();

// Configure custom TimeProvider (or FakeTimeProvider for testing)
services.AddTimingClock(options =>
{
    options.Kind = DateTimeKind.Utc;
    options.TimeProvider = myCustomTimeProvider;
});
```

### Clock behavior

- `Now` returns the current time as `DateTime`
- `NowOffset` returns the current time as `DateTimeOffset`
- `Normalize(DateTime)` normalizes the input according to the configured `DateTimeKind`
- `Normalize(DateTimeOffset)` converts the input to UTC or local based on configuration
- `TimeProvider` (.NET 8+) returns the active `TimeProvider` (defaults to `TimeProvider.System`)

## API

- `IClock`
  - `DateTime Now { get; }`
  - `DateTimeOffset NowOffset { get; }`
  - `DateTime Normalize(DateTime dateTime)`
  - `DateTimeOffset Normalize(DateTimeOffset dateTime)`
  - `bool SupportsMultipleTimezone { get; }`
  - `DateTimeKind Kind { get; }`
  - `TimeProvider TimeProvider { get; }` *(.NET 8+)*
- `ClockOptions`
  - `DateTimeKind Kind { get; set; }`
  - `TimeProvider? TimeProvider { get; set; }` *(.NET 8+)*
- `ClockServiceCollectionExtensions`
  - `AddTimingClock(this IServiceCollection services, Action<ClockOptions>? configureOptions = null)`

## Notes

- The concrete `Clock` class is internal sealed and not publicly exposed in the package.
- Users should only depend on `IClock`.
