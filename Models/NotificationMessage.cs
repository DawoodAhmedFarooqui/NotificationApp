namespace NotificationApp.Models;

public class NotificationMessage
{
    public string Message { get; set; } = string.Empty;
    public NotificationLevel Level { get; set; } = NotificationLevel.Info;
}
