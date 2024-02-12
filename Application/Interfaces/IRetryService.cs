

namespace Application.Interfaces
{
    public interface IRetryService
    {
        Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action);
        Task ExecuteWithRetryAsync(Func<Task> action);
    }
}
