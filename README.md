# Dekanat.ScheduleSdk

.NET 10 SDK для роботи з JSON API експорту розкладу **ПС-Розклад** Національного університету нафти і газу.

Офіційна сторінка параметрів: [timetable_export.cgi](https://dekanat.nung.edu.ua/cgi-bin/timetable_export.cgi).

## Можливості

- Типізовані моделі відповіді з `System.Text.Json` та `[JsonPropertyName]`
- HTTP-клієнт `IPsRozkladClient` для всіх `req_type` у JSON
- Кодування відповіді: **UTF-8 за замовчуванням**, опційно Windows-1251
- Обробка кодів помилки API (`PsRozkladApiException`)
- Реєстрація через `Microsoft.Extensions.DependencyInjection`

## Встановлення

```bash
dotnet add package Dekanat.ScheduleSdk
```

Або додайте посилання на проєкт у monorepo:

```xml
<ProjectReference Include="..\src\Dekanat.ScheduleSdk\Dekanat.ScheduleSdk.csproj" />
```

## Швидкий старт

### Без DI

```csharp
using Dekanat.ScheduleSdk;
using Dekanat.ScheduleSdk.Enums;
using Dekanat.ScheduleSdk.Requests;

HttpClient httpClient = new() { BaseAddress = new Uri(PsRozkladClientOptions.DefaultBaseUrl) };
IPsRozkladClient client = PsRozkladClient.Create(httpClient);

// Перелік груп з ID
PsRozkladExport groups = await client.GetObjectListAsync(new ObjectListRequest
{
    Mode = RequestMode.Group,
    IncludeIds = true,
});

// Розклад групи (окремі стовпчики)
PsRozkladExport schedule = await client.GetScheduleAsync(new ScheduleRequest
{
    Mode = RequestMode.Group,
    ObjectId = "-1664",
    BeginDate = new DateOnly(2026, 2, 1),
    EndDate = new DateOnly(2026, 2, 28),
    TextFormat = ScheduleTextFormat.Separated,
});

foreach (ScheduleItem item in schedule.ScheduleItems ?? [])
{
    Console.WriteLine($"{item.GetDate():dd.MM.yyyy} #{item.LessonNumber} {item.Title} — {item.Teacher}");
}
```

### ASP.NET Core

```csharp
builder.Services.AddPsRozkladClient(options =>
{
    options.Encoding = TextEncodingMode.Utf8;
    options.RequestTimeout = TimeSpan.FromSeconds(60);
});
```

## Кодування

| Режим | Параметр API | Опис |
|--------|----------------|------|
| `TextEncodingMode.Utf8` (за замовч.) | `coding_mode=UTF8` | Рекомендовано для JSON |
| `TextEncodingMode.Windows1251` | `coding_mode=WINDOWS-1251` | Legacy |

Глобально — у `PsRozkladClientOptions.Encoding`. Для одного запиту — властивість `Encoding` у `ObjectListRequest`, `ScheduleRequest` тощо.

## Методи клієнта

| Метод | `req_type` | Опис |
|--------|------------|------|
| `GetObjectListAsync` | `obj_list` | Групи / викладачі / аудиторії |
| `GetScheduleAsync` | `rozklad` | Розклад за ID, назвою або `dep_name` |
| `GetFreeRoomsAsync` | `free_rooms_list` | Вільні аудиторії |
| `GetRoomTypesAsync` | `room_type_list` | Типи аудиторій |
| `SendAsync` | довільний | Низькорівневий доступ |

## Моделі

Коренева відповідь: `PsRozkladResponse` → `PsRozkladExport`.

| JSON | Модель C# |
|------|-----------|
| `departments` | `Department` |
| `blocks` | `Building` |
| `roz_items` | `ScheduleItem` |
| `objects` (room types) | `RoomType` |
| `free_rooms` | `FreeRoomsEntry` |
| `error` | `ApiErrorDetails` |

Поле `code` та `errorcode` десеріалізуються як `int` навіть якщо сервер повертає рядок.

## Помилки API

Якщо `ThrowOnApiError = true` (за замовчуванням), при `code != 0` кидається `PsRozkladApiException` з `ErrorCode` та `ErrorMessage`.

Коди з документації відображені в `ApiResultCode` (наприклад `-90` — об'єкт не знайдено).

## Тести

```bash
dotnet test                              # 76 тестів (52 unit + 24 integration)
dotnet test --filter Category=Unit       # без мережі
dotnet test --filter Category=Integration

set SKIP_PSROZKLAD_INTEGRATION_TESTS=1   # інтеграція одразу виходить
dotnet test
```

| Проєкт | Покриття |
|--------|----------|
| `Dekanat.ScheduleSdk.Tests` | JSON-моделі, валідація, query (mock HTTP), `ApiQueryBuilder` |
| `Dekanat.ScheduleSdk.IntegrationTests` | `obj_list`, `rozklad`, `free_rooms_list`, `room_type_list`, `SendAsync` |

## Структура репозиторію

```
src/Dekanat.ScheduleSdk/
tests/Dekanat.ScheduleSdk.Tests/
tests/Dekanat.ScheduleSdk.IntegrationTests/
```

## Ліцензія

Уточніть ліцензію для вашої організації.
