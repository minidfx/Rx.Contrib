namespace Rx.Contrib
{
    using System.Threading.Tasks;

    /// <summary>
    ///     Define the contract of a subscription that you can wait for.
    /// </summary>
    internal interface IAwaitableSubscription : IAsyncDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Waits for the subscription is completed or an error occured.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Threading.Tasks.Task" /> representing the outcome of the operation.
        /// </returns>
        Task Await();

        #endregion
    }
}