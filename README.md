# Timing

Thư viện nhỏ cho .NET để xử lý thời gian toàn ứng dụng với hỗ trợ chuẩn hóa UTC/local và đăng ký dependency injection.

## Tính năng

- Trừu tượng `IClock` để lấy thời gian hiện tại
- Hỗ trợ `DateTime` và `DateTimeOffset`
- Chuẩn hóa theo `DateTimeKind` được cấu hình
- Đăng ký vào `ServiceCollection` qua `AddTimingClock`
- Lớp `Clock` là internal, chỉ lộ ra API công khai `IClock`

## Cài đặt

Nếu dùng repository này làm tham chiếu dự án, đảm bảo ứng dụng tiêu thụ chạy trên .NET 6.0.

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

### Hành vi của clock

- `Now` trả về thời gian hiện tại dưới dạng `DateTime`
- `NowOffset` trả về thời gian hiện tại dưới dạng `DateTimeOffset`
- `Normalize(DateTime)` chuẩn hóa đầu vào theo `DateTimeKind` đã cấu hình
- `Normalize(DateTimeOffset)` chuyển đầu vào về UTC hoặc local tuỳ cấu hình

## API

- `IClock`
  - `DateTime Now { get; }`
  - `DateTimeOffset NowOffset { get; }`
  - `DateTime Normalize(DateTime dateTime)`
  - `DateTimeOffset Normalize(DateTimeOffset dateTime)`
- `ClockOptions`
  - `DateTimeKind Kind { get; set; }`
- `ClockServiceCollectionExtensions`
  - `AddTimingClock(this IServiceCollection services, Action<ClockOptions>? configureOptions = null)`

## Ghi chú

- Lớp cụ thể `Clock` là internal và không được hiển thị công khai trong package.
- Người dùng nên chỉ phụ thuộc vào `IClock`.
