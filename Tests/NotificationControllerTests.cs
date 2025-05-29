using Microsoft.AspNetCore.Mvc;
using Moq;
using NotificationApp.Controllers;
using NotificationApp.Models;
using NotificationApp.Services;
using System.Threading.Tasks;
using Xunit;

public class NotificationControllerTests
{
    [Fact]
    public async Task Post_RespectsRateLimit()
    {
        var limiter = new SlidingWindowRateLimiter(1, TimeSpan.FromMinutes(1));
        var mockService = new Mock<INotificationService>();
        var logger = Mock.Of<ILogger<NotificationController>>();
        var controller = new NotificationController(limiter, mockService.Object, logger);


        var msg = new NotificationMessage { Message = "Test", Level = NotificationLevel.Info };

        var result1 = await controller.Post(msg);
        var result2 = await controller.Post(msg);

        Assert.IsType<OkObjectResult>(result1);
        Assert.IsType<ObjectResult>(result2);
        Assert.Equal(429, ((ObjectResult)result2).StatusCode);
    }

    [Fact]
    public async Task Post_Forwards_Warning_Message()
    {
        var limiter = new SlidingWindowRateLimiter(5, TimeSpan.FromMinutes(1));
        var mockService = new Mock<INotificationService>();
        var logger = Mock.Of<ILogger<NotificationController>>();
        var controller = new NotificationController(limiter, mockService.Object, logger);

        var msg = new NotificationMessage { Message = "Warn me", Level = NotificationLevel.Warning };

        var result = await controller.Post(msg);

        Assert.IsType<OkObjectResult>(result);
    }
}
