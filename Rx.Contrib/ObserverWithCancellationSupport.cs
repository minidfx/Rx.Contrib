namespace Rx.Contrib
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     Observer which holds a task completion source which can be used to request observable completion.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The source type.
    /// </typeparam>
    internal sealed class ObserverWithCancellationSupport<TSource> : IObserver<TSource>
    {
        #region Constructors and Destructors

        public ObserverWithCancellationSupport(CancellationToken token,
                                               TaskCompletionSource<object> taskCompletionSource,
                                               Action<TSource> onNext = null,
                                               Action<Exception> onError = null,
                                               Action onCompleted = null)
        {
            this.Token = token;
            this.taskCompletionSource = taskCompletionSource;

            this.onNext = onNext ?? (o => { });
            this.onError = onError ?? (ex => { });
            this.onCompleted = onCompleted ?? (() => { });
        }

        #endregion

        #region Fields

        private readonly Action onCompleted;

        private readonly Action<Exception> onError;

        private readonly Action<TSource> onNext;

        private readonly TaskCompletionSource<object> taskCompletionSource;

        #endregion

        #region Public Properties

        public bool IsCompleted { get; set; }

        public CancellationToken Token { get; }

        #endregion

        #region Public Methods and Operators

        public void OnCompleted()
        {
            this.IsCompleted = true;
            this.taskCompletionSource.SetResult(0);
            this.onCompleted();
        }

        public void OnError(Exception error)
        {
            this.taskCompletionSource.SetException(error);
            this.onError(error);
        }

        public void OnNext(TSource value)
        {
            this.onNext(value);
        }

        #endregion
    }
}