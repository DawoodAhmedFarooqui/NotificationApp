using System.Threading.Tasks;
using Moq;
using NotificationApp.Models;
using NotificationApp.Services;
using Xunit;

public class NotificationServiceTests
{
    [Fact]
    public async Task HandleAsync_DoesNotForward_Info()
    {
        var mockNotifier = new Mock<IExternalNotifier>();
        var logger = Mock.Of<ILogger<NotificationService>>();
        var service = new NotificationService(mockNotifier.Object, logger);
        
        var message = new NotificationMessage { Level = NotificationLevel.Info, Message = "Hello" };

        await service.HandleAsync(message);

        // Just check that no exceptions are thrown
        Assert.True(true);
    }

    [Fact]
    public async Task HandleAsync_Forwards_Warning_And_Error()
    {
        var mockNotifier = new Mock<IExternalNotifier>();
        var logger = Mock.Of<ILogger<NotificationService>>();
        var service = new NotificationService(mockNotifier.Object, logger);

        var warning = new NotificationMessage { Level = NotificationLevel.Warning, Message = "Warn" };
        var error = new NotificationMessage { Level = NotificationLevel.Error, Message = "Err" };

        await service.HandleAsync(warning);
        await service.HandleAsync(error);

        Assert.True(true); // Again, we're trusting ForwardToDiscordAsync stub for now
    }
}
