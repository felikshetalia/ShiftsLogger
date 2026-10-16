public sealed class ApiClientException : Exception
{
    public ApiClientException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
