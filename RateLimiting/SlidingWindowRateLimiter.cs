public class SlidingWindowRateLimiter
{
    private readonly int _limit;
    private readonly TimeSpan _window;
    private readonly Queue<DateTime> _timestamps = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public SlidingWindowRateLimiter(int limit, TimeSpan window)
    {
        _limit = limit;
        _window = window;
    }

    public async Task<bool> AllowRequestAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            var now = DateTime.UtcNow;

            // Remove expired timestamps
            while (_timestamps.Count > 0 && now - _timestamps.Peek() > _window)
            {
                _timestamps.Dequeue();
            }

            if (_timestamps.Count >= _limit)
                return false;

            _timestamps.Enqueue(now);
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
