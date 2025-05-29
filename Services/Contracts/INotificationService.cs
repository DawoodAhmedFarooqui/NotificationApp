using NotificationApp.Models;

namespace NotificationApp.Services;

public interface INotificationService
{
    Task HandleAsync(NotificationMessage msg);
}