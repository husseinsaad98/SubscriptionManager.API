using Application.Interfaces;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Infrastructure.Services
{
    public class RetryService : IRetryService
    {
        public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action)
        {
            var policy = Policy.Handle<Exception>()
                               .WaitAndRetryAsync(
                                    Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5),
                                    (exception, retryCount) =>
                                    {
                                        Console.WriteLine($"{retryCount} count");
                                    });

            return await policy.ExecuteAsync(action);
        }

        public async Task ExecuteWithRetryAsync(Func<Task> action)
        {
            var policy = Policy.Handle<Exception>()
                                .WaitAndRetryAsync(
                                     Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5),
                                     (exception, retryCount) =>
                                     {
                                         Console.WriteLine($"{retryCount} count");
                                     });

            await policy.ExecuteAsync(action);
        }
    }
}
