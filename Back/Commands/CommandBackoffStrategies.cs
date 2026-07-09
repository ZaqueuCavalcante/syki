namespace Estud.Back.Commands;

public static class CommandBackoffStrategies
{
    public static int? GetDelaySeconds(BackoffStrategy strategy, int baseDelaySeconds, int retryAttempt)
    {
        return strategy switch
        {
            BackoffStrategy.None => null,
            BackoffStrategy.Exponential => Exponential(baseDelaySeconds, retryAttempt),
            BackoffStrategy.Linear => Linear(baseDelaySeconds, retryAttempt),
            BackoffStrategy.Fixed => Fixed(baseDelaySeconds),
            _ => null,
        };
    }

    /// <summary>
    /// base*1, base*2, base*4, base*8...  (baseDelay * 2^(retryAttempt-1))
    /// </summary>
    private static int Exponential(int baseDelaySeconds, int retryAttempt)
    {
        return baseDelaySeconds * (int)Math.Pow(2, retryAttempt - 1);
    }

    /// <summary>
    /// base*1, base*2, base*3, base*4...  (baseDelay * retryAttempt)
    /// </summary>
    private static int Linear(int baseDelaySeconds, int retryAttempt)
    {
        return baseDelaySeconds * retryAttempt;
    }

    /// <summary>
    /// base, base, base...
    /// </summary>
    private static int Fixed(int baseDelaySeconds)
    {
        return baseDelaySeconds;
    }
}
