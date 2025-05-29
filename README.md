# Notification API

A simple ASP.NET Core Web API that accepts notification messages over HTTP and forwards important ones (like warnings and errors) to an external service (e.g., Discord). Includes rate limiting (10 messages per minute), structured logging, and unit tests.

This project is designed to be **scalable and extensible**. While it currently uses a **Sliding Window Rate Limiting** algorithm for better scalability, the architecture can easily switch to other algorithms like **Fixed Window**, as per modular design. For each new request, the rate limiter removes entries older than the time window and checks if the number of recent requests exceeds the configured limit.

Additionally, the system follows the **Interface Segregation Principle** so that support for additional external notification services (e.g., Slack, Email) can be added easily by implementing a new notifier interface.

---

## Features

- Accepts messages via HTTP `POST` request
- Supports message `Level`: `Info`, `Warning`, `Error`
- Forwards `Warning` and `Error` messages to external service (e.g., Discord)
- Skips rate limiting for high-priority messages (`Warning` and `Error`)
- Rate limiting for `Info` messages: max 10 messages per minute
- Uses **Sliding Window Algorithm** for scalable rate limiting
- Clean, testable architecture with interfaces
- Structured logging
- Unit tests using **xUnit**

---

## Technologies Used

- ASP.NET Core Web API
- C# 10 / .NET 8
- xUnit (unit testing)
- Moq (mocking dependencies)

---

## Message Payload Format

### POST `/notification`

```json
{
  "message": "Disk is almost full!",
  "level": 1 // Warning
}
```

### `NotificationLevel` Enum

```csharp
public enum NotificationLevel
{
    Info,
    Warning,
    Error
}
```

---

## Rate Limiting

Implements a **Sliding Window Algorithm** that allows **10 requests per 60 seconds**.

```csharp
new SlidingWindowRateLimiter(maxRequests: 10, window: TimeSpan.FromMinutes(1));
```

- 11th request within the same minute gets an HTTP `429 Too Many Requests`.

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio or Rider IDE

### Run the App

```bash
dotnet run
```

The app will be running at: `https://localhost:5293`

---

## Running Tests

Run tests with:

```bash
dotnet test
```

This will run:

- `SlidingWindowRateLimiter` unit tests
- `NotificationService` behavior tests
- `NotificationController` integration logic tests

---

## Project Structure

```
/Controllers
    NotificationController.cs
/Models
    NotificationMessage.cs
    NotificationLevel.cs
/Services
    NotificationService.cs
    /Contracts
        INotificationService.cs
/Notifiers
    DiscordNotifier.cs
    /Contracts
        IExternalNotifiers.cs
/RateLimiting
    SlidingWindowRateLimiter.cs
/Tests
    NotificationControllerTests.cs
    NotificationServiceTests.cs
    SlidingWindowRateLimiterTests.cs
```

---

## Forwarding to External Systems

The app currently logs forwarded messages to the console. You can integrate real services like:

- Discord Webhooks
- Slack
- Email


---

## Example cURL Request

```bash
curl --location 'http://localhost:5293/notification' \
--header 'Content-Type: application/json' \
--data '{
    "message": "Disk is almost full!",
    "level": 0
}'
```

- `level: 0` corresponds to `NotificationLevel.Info`
- You can also use `1` for `Warning` and `2` for `Error`