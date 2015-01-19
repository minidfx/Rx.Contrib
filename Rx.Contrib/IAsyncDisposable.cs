namespace Rx.Contrib
{
    using System.Threading.Tasks;

    /// <summary>
    ///   Defines an asynchronous method to release allocated resources.
    /// </summary>
    public interface IAsyncDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        ///   asynchronously.
        /// </summary>
        /// <returns>
        ///   A <see cref="Task" /> representing the outcome of the operation.
        /// </returns>
        Task DisposeAsync();

        #endregion
    }
}