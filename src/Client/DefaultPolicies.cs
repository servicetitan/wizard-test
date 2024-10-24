using Polly;

namespace ServiceTitan.WizardTest.Client;

internal static class DefaultPolicies
{
    private const int DefaultMaxRetries = 5;

    public static IAsyncPolicy<T> RetryWithinTimeoutPolicy<T>(TimeSpan timeout) =>
        Policy
            .TimeoutAsync<T>(timeout)
            .WrapAsync<T>(
                Policy<T>
                    .Handle<Exception>()
                    .WaitAndRetryAsync(DefaultMaxRetries, GetExponentionalBackoffWithJitter));

    //Default delay maximum no more than 5 seconds
    private static TimeSpan GetExponentionalBackoffWithJitter(int attempt) =>
        TimeSpan.FromMilliseconds(Math.Pow(5, attempt)) + TimeSpan.FromMilliseconds(Random.Shared.Next(1000));
}
