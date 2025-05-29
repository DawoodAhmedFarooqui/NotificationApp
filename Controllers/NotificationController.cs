using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using NotificationApp.Services;

namespace NotificationApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly SlidingWindowRateLimiter _rateLimiter;
        private readonly INotificationService _service;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(SlidingWindowRateLimiter limiter, INotificationService service, ILogger<NotificationController> logger)
        {
            _rateLimiter = limiter;
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NotificationMessage message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Message))
                return BadRequest("Invalid message");

            if (message.Level >= NotificationLevel.Warning)
            {
                await _service.HandleAsync(message);  
                return Ok("Message forwarded immediately to discord.");
            }

            if (!await _rateLimiter.AllowRequestAsync())
            {
                return StatusCode(429, "Rate limit exceeded. Max 10 requests per 60 seconds.");
            }
            
            await _service.HandleAsync(message);
            
            return Ok("Message received.");
        }
    }
}
