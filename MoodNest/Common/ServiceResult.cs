namespace MoodNest.Common;

/// <summary>
/// Represents a standardized result wrapper for service-layer operations.
/// Encapsulates success status, returned data, and error information.
/// </summary>
/// <typeparam name="T">Type of data returned by the service.</typeparam>
public class ServiceResult<T>
{
    /// <summary>
    /// Indicates whether the operation completed successfully.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Data returned by the operation when successful.
    /// Will be null if the operation fails.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Error message describing the failure.
    /// Empty when the operation is successful.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Creates a successful service result containing data.
    /// </summary>
    /// <param name="data">The data produced by the operation.</param>
    /// <returns>A successful ServiceResult instance.</returns>
    public static ServiceResult<T> SuccessResult(T data)
    {
        return new ServiceResult<T>
        {
            Success = true,
            Data = data
        };
    }

    /// <summary>
    /// Creates a failed service result with an error message.
    /// </summary>
    /// <param name="error">Description of the failure.</param>
    /// <returns>A failed ServiceResult instance.</returns>
    public static ServiceResult<T> FailureResult(string error)
    {
        return new ServiceResult<T>
        {
            Success = false,
            ErrorMessage = error
        };
    }
}