using System;
using System.Threading.Tasks;
using Xunit;

public class SlidingWindowRateLimiterTests
{
    [Fact]
    public async Task AllowRequestAsync_Allows_Limited_Requests()
    {
        var limiter = new SlidingWindowRateLimiter(10, TimeSpan.FromMinutes(1));

        // Should allow 10 requests
        for (int i = 0; i < 10; i++)
        {
            var allowed = await limiter.AllowRequestAsync();
            Assert.True(allowed, $"Request {i + 1} should be allowed.");
        }

        // 11th request should be denied
        var denied = await limiter.AllowRequestAsync();
        Assert.False(denied, "11th request should be denied.");
    }

    [Fact]
    public async Task AllowRequestAsync_Allows_After_Window_Passes()
    {
        var limiter = new SlidingWindowRateLimiter(2, TimeSpan.FromSeconds(1));

        Assert.True(await limiter.AllowRequestAsync());
        Assert.True(await limiter.AllowRequestAsync());
        Assert.False(await limiter.AllowRequestAsync());

        await Task.Delay(1100); // wait for 1.1 seconds

        Assert.True(await limiter.AllowRequestAsync()); // should allow again
    }

    [Fact]
    public async Task AllowRequest_AllowsUpToLimit()
    {
        var limiter = new SlidingWindowRateLimiter(3, TimeSpan.FromSeconds(10));
        Assert.True(await limiter.AllowRequestAsync());
        Assert.True(await limiter.AllowRequestAsync());
        Assert.True(await limiter.AllowRequestAsync());
        Assert.False(await limiter.AllowRequestAsync());
    }
}
