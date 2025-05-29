using NotificationApp.Models;

namespace NotificationApp.Services;

public class DiscordNotifier : IExternalNotifier
{
    public Task ForwardAsync(NotificationMessage message)
    {
        Console.WriteLine($"Forwarded to Discord: {message.Message} [{message.Level}]");
        return Task.CompletedTask;
    }
}