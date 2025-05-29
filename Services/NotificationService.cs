using NotificationApp.Models;

namespace NotificationApp.Services;

public class NotificationService : INotificationService
{
    private readonly IExternalNotifier _notifier;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IExternalNotifier notifier, ILogger<NotificationService> logger)
    {
        _notifier = notifier;
        _logger = logger;
    }

    public async Task HandleAsync(NotificationMessage msg)
    {
        _logger.LogInformation("Received: {Message} [{Level}]", msg.Message, msg.Level);

        if (msg.Level >= NotificationLevel.Warning)
        {
            await _notifier.ForwardAsync(msg);
        }
    }
}
