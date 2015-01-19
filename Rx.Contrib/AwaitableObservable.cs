namespace Rx.Contrib
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    internal class AwaitableObservable<TSource> : IAsyncDisposable
    {
        #region Fields

        private readonly IDisposable subscription;

        private readonly TaskCompletionSource<TSource> tcs;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableObservable{T}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="subscribeAction">
        ///   The function which will be executed to retrieve the subscription on the <paramref name="source" />.
        /// </param>
        public AwaitableObservable(IObservable<TSource> source,
                                   Func<IObservable<TSource>, IDisposable> subscribeAction)
            : this()
        {
            this.subscription = subscribeAction(source.Finally(() => this.tcs.SetResult(default(TSource))));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableObservable{T}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="subscribeAction">
        ///   The action which will be executed to execute the subscription on the <paramref name="source" />.
        /// </param>
        public AwaitableObservable(IObservable<TSource> source,
                                   Action<IObservable<TSource>> subscribeAction)
            : this()
        {
            subscribeAction(source.Finally(() => this.tcs.SetResult(default(TSource))));
        }

        private AwaitableObservable()
        {
            this.tcs = new TaskCompletionSource<TSource>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        ///   asynchronously.
        /// </summary>
        /// <returns>
        ///   A <see cref="T:System.Threading.Tasks.Task" /> representing the outcome of the operation.
        /// </returns>
        public Task DisposeAsync()
        {
            if (this.subscription != null)
            {
                this.subscription.Dispose();
            }

            return this.tcs.Task;
        }

        #endregion
    }
}