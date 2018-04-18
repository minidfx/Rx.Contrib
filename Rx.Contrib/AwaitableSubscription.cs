namespace Rx.Contrib
{
    using System;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     Provides a set of static methods for subscribing delegates to observable and add the possbility
    ///     for waiting the end of the subcription, occured when the observable is completed or an error is produced.
    /// </summary>
    /// <remarks>
    ///     Don't forget the Rx grammer for asynchronous sequences of data : OnNext* (OnCompleted|OnError)?
    /// </remarks>
    /// <typeparam name="TSource">
    ///     The type of the elements in the source sequence.
    /// </typeparam>
    internal class AwaitableSubscription<TSource> : IAwaitableSubscription
    {
        #region Fields

        private readonly IDisposable subscription;

        private readonly TaskCompletionSource<TSource> tcs;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableSubscription{TSource}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        public AwaitableSubscription(IObservable<TSource> source)
            : this()
        {
            var newSource = source.Finally(() => this.tcs.SetResult(default(TSource)));
            this.subscription = newSource.Subscribe();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableSubscription{TSource}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        public AwaitableSubscription(IObservable<TSource> source,
                                     Action<TSource> onNext)
            : this()
        {
            var newSource = source.Finally(() => this.tcs.SetResult(default(TSource)));
            this.subscription = newSource.Subscribe(onNext);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableSubscription{TSource}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///     Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        public AwaitableSubscription(IObservable<TSource> source,
                                     Action<TSource> onNext,
                                     Action<Exception> onError)
            : this()
        {
            var newSource = source.Finally(() => this.tcs.SetResult(default(TSource)));
            this.subscription = newSource.Subscribe(onNext, onError);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableSubscription{TSource}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onCompleted">
        ///     Action to invoke upon graceful termination of the observable sequence.
        /// </param>
        public AwaitableSubscription(IObservable<TSource> source,
                                     Action<TSource> onNext,
                                     Action onCompleted)
            : this()
        {
            var newSource = source.Finally(() => this.tcs.SetResult(default(TSource)));
            this.subscription = newSource.Subscribe(onNext, onCompleted);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableSubscription{TSource}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///     Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        /// <param name="onCompleted">
        ///     Action to invoke upon graceful termination of the observable sequence.
        /// </param>
        public AwaitableSubscription(IObservable<TSource> source,
                                     Action<TSource> onNext,
                                     Action<Exception> onError,
                                     Action onCompleted)
            : this()
        {
            var newSource = source.Finally(() => this.tcs.SetResult(default(TSource)));
            this.subscription = newSource.Subscribe(onNext, onError, onCompleted);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableSubscription{TSource}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="observer">
        ///     Observer to subscribe to the sequence.
        /// </param>
        /// <param name="token">
        ///     CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        public AwaitableSubscription(IObservable<TSource> source,
                                     IObserver<TSource> observer,
                                     CancellationToken token)
            : this()
        {
            var newSource = source.Finally(() => this.tcs.SetResult(default(TSource)));
            newSource.Subscribe(observer, token);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableSubscription{TSource}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="token">
        ///     CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        public AwaitableSubscription(IObservable<TSource> source,
                                     CancellationToken token)
            : this()
        {
            var newSource = source.Finally(() => this.tcs.SetResult(default(TSource)));
            newSource.Subscribe(token);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableSubscription{TSource}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///     Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        /// <param name="token">
        ///     CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        public AwaitableSubscription(IObservable<TSource> source,
                                     Action<TSource> onNext,
                                     Action<Exception> onError,
                                     CancellationToken token)
            : this()
        {
            var newSource = source.Finally(() => this.tcs.SetResult(default(TSource)));
            newSource.Subscribe(onNext, onError, token);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AwaitableSubscription{TSource}" /> class.
        /// </summary>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///     Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        /// <param name="onCompleted">
        ///     Action to invoke upon graceful termination of the observable sequence.
        /// </param>
        /// <param name="token">
        ///     CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        public AwaitableSubscription(IObservable<TSource> source,
                                     Action<TSource> onNext,
                                     Action<Exception> onError,
                                     Action onCompleted,
                                     CancellationToken token)
            : this()
        {
            var newSource = source.Finally(() => this.tcs.SetResult(default(TSource)));
            newSource.Subscribe(onNext, onError, onCompleted, token);
        }

        private AwaitableSubscription()
        {
            this.tcs = new TaskCompletionSource<TSource>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Waits for the subscription is completed or an error occured.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Threading.Tasks.Task" /> representing the outcome of the operation.
        /// </returns>
        public Task Await()
        {
            return this.tcs.Task;
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        ///   asynchronously.
        /// </summary>
        /// <returns>
        ///   A <see cref="T:System.Threading.Tasks.Task" /> representing the outcome of the operation.
        /// </returns>
        public Task DisposeAsync()
        {
            this.subscription?.Dispose();

            return this.tcs.Task;
        }

        #endregion
    }
}