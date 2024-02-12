namespace Application.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }
        public string SuccessMessage { get; set; }
        public static Result<T> Success(T value, string successMessage = "") => new()
        {
            IsSuccess = true,
            Value = value,
            SuccessMessage = successMessage
        };
        public static Result<T> Failure(string error) => new()
        {
            IsSuccess = false,
            Error = error
        };
    }
}
