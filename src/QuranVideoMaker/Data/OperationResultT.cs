namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Represents the result of an operation with additional data
    /// </summary>
    /// <typeparam name="T">The type of data returned</typeparam>
    public class OperationResult<T> : OperationResult
    {
        /// <summary>
        /// The data returned by the operation
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult" /> class.
        /// </summary>
        /// <param name="success">Whether the operation was successful.</param>
        /// <param name="message">The status of the operation.</param>
        /// <param name="data">The data.</param>
        public OperationResult(bool success, string message, T data) : base(success, message)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
