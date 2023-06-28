namespace QuranVideoMaker.Data
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a message indicating the status of the operation.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="success">Whether the operation was successful.</param>
        /// <param name="message">The status of the operation.</param>
        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
