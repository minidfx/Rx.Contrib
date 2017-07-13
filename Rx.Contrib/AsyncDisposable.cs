namespace Rx.Contrib
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     Represents an asynchronous Func{Task}-based disposable and contains helpers methods to work with <see cref="IAsyncDisposable"/>.
    /// </summary>
    public class AsyncDisposable : IAsyncDisposable
    {
        #region Fields

        private Func<Task> disposeTask;

        #endregion

        #region Constructors and Destructors

        private AsyncDisposable(
            Func<Task> disposeTask)
        {
            this.disposeTask = disposeTask;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates a asynchronous disposable object that invokes the specified operation when disposed.
        /// </summary>
        /// <param name="disposeAsync">
        ///     Operation to run during the first call to <see cref="IAsyncDisposable.DisposeAsync"/>. 
        ///     The operation is guaranteed to be run at most once.
        /// </param>
        /// <returns>
        ///     The disposable object that runs the given function upon disposal.
        /// </returns>
        public static IAsyncDisposable Create(
            Func<Task> disposeAsync)
        {
            return new AsyncDisposable(disposeAsync);
        }

        /// <summary>
        ///     Calls the disposal function if and only if the current instance hasn't been disposed yet.
        /// </summary>
        /// <returns>
        ///   A <see cref="Task"/> representing the outcome of the operation.
        /// </returns>
        public Task DisposeAsync()
        {
            var localDisposeTask = Interlocked.Exchange(ref this.disposeTask, null);

            var task = localDisposeTask == null
                           ? Task.CompletedTask
                           : localDisposeTask();

            return task;
        }

        #endregion
    }
}