using NotificationApp.Models;

namespace NotificationApp.Services;

public interface IExternalNotifier
{
    Task ForwardAsync(NotificationMessage message);
}